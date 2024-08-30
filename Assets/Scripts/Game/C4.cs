using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : Weapon
{
    [SerializeField] private GameObject _model;
    [SerializeField] private BoxCollider _physicsCollider;
    public bool BombDown { get; set; }
    private PlayerBridge _bridge;

    private float _plantingTime = 3f;

    public override void Fire()
    {
        if (BombDown || _bridge == null) return;
    }

    private void Start()
    {
        BombDown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Thing entered trigger");
        if (other.CompareTag("Player"))
        {
            _bridge = other.GetComponent<PlayerBridge>();
            if (_bridge.player.PlayerTeam == Player.PlayerTeams.T)
            {
                _bridge.player.ownedWeapons.HasBomb = true;
                _physicsCollider.enabled = false;
                BombDown = false;
            }
        }
    }

    private void Update()
    {
        if (!BombDown)
        {
            transform.position = _bridge.PlayerInventory.BombHolder.transform.position;
        }
    }

    private void PickupBomb()
    {
        // BombDown = false;
        // _model.SetActive(false);
    }
}
