using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBridge : MonoBehaviour
{
    // For all values that need to be accessed by EACH sub-set
    [Header("Cameras")] 
    public GameObject cameraHolder;
    public Camera playerCamera;
    public Camera weaponCamera;
    public Camera uiCamera;
    
    [Header("Player Models")]
    public GameObject[] playerModels;
}
