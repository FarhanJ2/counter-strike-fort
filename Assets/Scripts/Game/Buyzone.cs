using System;
using UnityEngine;

public class Buyzone : MonoBehaviour
{
    public static event Action OnExitBuyzone;
    public Player.PlayerTeams BuyzoneTeam;
    
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player.PlayerTeam == BuyzoneTeam || BuyzoneTeam == Player.PlayerTeams.UNASSIGNED)
        {
            player.InBuyZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        player.InBuyZone = false;
        OnExitBuyzone?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
    }
}
