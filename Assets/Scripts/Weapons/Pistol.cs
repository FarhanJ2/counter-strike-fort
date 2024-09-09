using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Fire(PlayerBridge bridge)
    {
        Debug.Log("Firing pistol");
    }
}
