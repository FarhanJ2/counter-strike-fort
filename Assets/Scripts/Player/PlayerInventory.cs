using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    public GameObject BombHolder;
    private PlayerBridge _bridge;
    
    private Weapon.WeaponName _slot1, _slot2, _slot3, _slot4, _slot5;
    private Weapon.WeaponName _currentWeaponHolding;
    // public Weapon.WeaponName CurrentWeaponHolding
    // {
    //     get => _currentWeaponHolding;
    //     set => SetHoldingWeapon(value);
    // }

    private void Start()
    {
        _bridge = GetComponent<PlayerBridge>();
        _bridge.InputManager.PlayerControls.Inventory.Slot1.started += _ => SetHoldingWeapon(1);
        _bridge.InputManager.PlayerControls.Inventory.Slot2.started += _ => SetHoldingWeapon(2);
        _bridge.InputManager.PlayerControls.Inventory.Slot3.started += _ => SetHoldingWeapon(3);
        _bridge.InputManager.PlayerControls.Inventory.Slot4.started += _ => SetHoldingWeapon(4);
        _bridge.InputManager.PlayerControls.Inventory.Slot5.started += _ => SetHoldingWeapon(5);
    }

    private void SetHoldingWeapon(int slot)
    {
        foreach (GameObject w in _weapons)
        {
            w.SetActive(false);
        }

        if (_bridge.player.ownedWeapons.CurrentPrimary == Weapon.WeaponName.NONE || _bridge.player.ownedWeapons.CurrentSecondary == Weapon.WeaponName.NONE || !_bridge.player.ownedWeapons.HasBomb)
        {
            Debug.Log("Player doesnt own any weapon in slot " + slot);
            return;
        }
        
        switch (slot)
        {
            case 1:
                
                break;
            case 2:
            case 3:
            case 4:
            case 5:
                // _bomb.SetActive(true);
                break;
        }
    }
}
