using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : NetworkBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    public GameObject BombHolder;
    private PlayerBridge _bridge;
    private Camera _viewCam;
    
    private Weapon.WeaponName _slot1, _slot2, _slot3, _slot4, _slot5;
    private Weapon.WeaponName _currentWeaponHolding;

    [SerializeField] private LayerMask _pickupLayer;
    private float raycastDistance = 4f;
    
    private Action<InputAction.CallbackContext> _fireAction;
    
    // public Weapon.WeaponName CurrentWeaponHolding
    // {
    //     get => _currentWeaponHolding;
    //     set => SetHoldingWeapon(value);
    // }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            enabled = false;
        }

        _viewCam = _bridge.playerCamera;
    }

    private void Start()
    {
        _bridge = GetComponent<PlayerBridge>();
        _bridge.InputManager.PlayerControls.Inventory.Slot1.started += _ => SetHoldingWeapon(1);
        _bridge.InputManager.PlayerControls.Inventory.Slot2.started += _ => SetHoldingWeapon(2);
        _bridge.InputManager.PlayerControls.Inventory.Slot3.started += _ => SetHoldingWeapon(3);
        _bridge.InputManager.PlayerControls.Inventory.Slot4.started += _ => SetHoldingWeapon(4);
        _bridge.InputManager.PlayerControls.Inventory.Slot5.started += _ => SetHoldingWeapon(5);

        _bridge.InputManager.PlayerControls.Inventory.Pickup.started += _ => Pickup();
        _bridge.InputManager.PlayerControls.Inventory.Drop.started += _ => DropItem();
    }

    private void DropItem()
    {
        if (_currentWeaponHolding ==
            Weapon.WeaponName.NONE /* || _currentWeaponHolding == put logic to not throw knife */)
        {
            Debug.Log("No weapon is being held");
            return;
        }
            
        
        DropObjectServer(_currentWeaponHolding, this);
    }

    public void DropItem(Weapon.WeaponName weapon)
    {
        DropObjectServer(weapon, this);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropObjectServer(Weapon.WeaponName weapon, PlayerInventory inv)
    {
        DropObjectObserver(weapon, inv);
    }

    [ObserversRpc]
    private void DropObjectObserver(Weapon.WeaponName weapon, PlayerInventory inv)
    {
        if (inv._bridge == null) // return if not owner
        {
            return;
        }
        
        SetHoldingWeapon(-1); // -1 used to disarm player, player is holding no weapons
        
        if (weapon == Weapon.WeaponName.C4)
        {
            C4 c4 = FindObjectOfType<C4>();
            inv._bridge.player.ownedWeapons.HasBomb = false;
            c4.DropBomb();
        }
        
        inv._currentWeaponHolding = Weapon.WeaponName.NONE;
        // obj.transform.parent = null;
        //
        // if (obj.GetComponent<Rigidbody>() != null)
        // {
        //     obj.GetComponent<Rigidbody>().isKinematic = false;
        // }
    }
    
    private void Pickup()
    {
        if (Physics.Raycast(_viewCam.transform.position, _viewCam.transform.forward, out RaycastHit hit,
                raycastDistance, _pickupLayer))
        {
            PickupObjectServer(hit.transform.gameObject,gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PickupObjectServer(GameObject obj, GameObject player)
    {
        SetPickupObjectObserver(obj, player);   
    }

    [ObserversRpc]
    private void SetPickupObjectObserver(GameObject obj, GameObject player)
    {
        
    }

    private void SetHoldingWeapon(int slot)
    {
        if (!IsOwner) return;

        _currentWeaponHolding = Weapon.WeaponName.NONE;
        _bridge.InputManager.PlayerControls.Attack.Fire.started -= _fireAction;
        
        foreach (GameObject w in _weapons)
        {
            w.SetActive(false);
        }

        if ((_bridge.player.ownedWeapons.CurrentPrimary == Weapon.WeaponName.NONE && slot == 1) ||
             (_bridge.player.ownedWeapons.CurrentSecondary == Weapon.WeaponName.NONE && slot == 2) || (!_bridge.player.ownedWeapons.HasBomb && slot == 5) || slot == -1)
        {
            Debug.Log("Player doesnt own any weapon in slot " + slot);
            return;
        }
        
        switch (slot)
        {
            case 1:
                _currentWeaponHolding = _bridge.player.ownedWeapons.CurrentPrimary;
                _weapons[2].SetActive(true);
                _fireAction = ctx => OnFireWeapon(_weapons[2].GetComponent<Rifle>());
                _bridge.InputManager.PlayerControls.Attack.Fire.started += _fireAction;
                break;
            case 2:
                _currentWeaponHolding = _bridge.player.ownedWeapons.CurrentSecondary;
                _weapons[1].SetActive(true);
                _fireAction = ctx => OnFireWeapon(_weapons[1].GetComponent<Pistol>());
                _bridge.InputManager.PlayerControls.Attack.Fire.started += _fireAction;
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                _currentWeaponHolding = Weapon.WeaponName.C4;
                C4 c4 = FindObjectOfType<C4>();
                c4.GetComponentInChildren<MeshRenderer>().enabled = false;
                c4.GetComponent<BoxCollider>().enabled = false;
                _weapons[0].SetActive(true);
                _fireAction = ctx => OnFireWeapon(c4);
                _bridge.InputManager.PlayerControls.Attack.Fire.started += _fireAction;
                _bridge.InputManager.PlayerControls.Attack.Fire.canceled += _ =>
                {
                    c4.StopPlanting(_bridge);
                };
                break;
        }
    }

    private void OnFireWeapon(Weapon weapon)
    {
        weapon.Fire(_bridge);
    }
}
