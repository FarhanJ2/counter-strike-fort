using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBridge : NetworkBehaviour
{
    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //     if (!base.IsOwner)
    //     {
    //         gameObject.GetComponent<PlayerBridge>().enabled = false;
    //     }
    // }

    // For all values that need to be accessed by EACH sub-set
    [Header("Player Files")] 
    public PlayerMovement playerMovement;
    public Player player;
    public PlayerCam PlayerCam;
    public InputManager InputManager;
    public PlayerSounds playerSounds;
    public PlayerInventory PlayerInventory;

    [Header("UI Managers")] 
    public UIHud uiHud;
    
    [Header("Cameras")] 
    public GameObject cameraHolder;
    public Camera playerCamera;
    public Camera weaponCamera;
    public Camera uiCamera;
    
    [Header("Player Models")]
    public GameObject[] playerModels;
}
