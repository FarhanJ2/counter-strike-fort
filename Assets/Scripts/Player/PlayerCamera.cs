using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
    }
}
