using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    private float _nextFireTime = 0f;
    
    public override void Fire()
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable()
    {
        // InputManager.Instance.PlayerControls.Movement.Crouch.started += _ => Crouch();
        InputManager.Instance.PlayerControls.Attack.Reload.started += _ => StartCoroutine(Reload());
    }

    private void Update()
    {
        if (isReloading) return;

        if (isAutomatic)
        {
            if (Input.GetMouseButton(1) && Time.time >= _nextFireTime)
            {
                _nextFireTime = Time.time + 1f / fireRate;
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= _nextFireTime)
                {

                }
            }
        }
    }
}
