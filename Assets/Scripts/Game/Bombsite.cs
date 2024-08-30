using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombsite : MonoBehaviour
{
    public bool BombPlanted { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player =other.GetComponent<Player>();
            player.InBombZone = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player =other.GetComponent<Player>();
            player.InBombZone = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
    }
}
