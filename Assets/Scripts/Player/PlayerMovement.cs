using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Example.ColliderRollbacks;
using FishNet.Object;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float jumpHeight = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    [SerializeField] private Camera playerCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            playerCamera = Camera.main;
            
        }
        else
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
