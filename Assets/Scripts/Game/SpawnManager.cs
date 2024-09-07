using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private List<Spawn> _ctSpawns, _tSpawns;
    public static SpawnManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetFreeSpawnServer(Player player)
    {
        Debug.Log("Spawner server ran");
        GetFreeSpawnObserver(player);
    }

    [ObserversRpc]
    private void GetFreeSpawnObserver(Player player)
    {
        if (_ctSpawns == null) Debug.Log("CT is null");
        if (_tSpawns == null) Debug.Log("T is null");
        Debug.Log(player.PlayerTeam);
        
        player.transform.position = GetFreeSpawn(player.PlayerTeam);
    }
    
    private Vector3 GetFreeSpawn(Player.PlayerTeams team)
    {
        Debug.Log("Get spawn ran");
        foreach (Spawn spawns in team == Player.PlayerTeams.CT ? _ctSpawns : _tSpawns)
        {
            Debug.Log(spawns);
            if (!spawns.IsOccupied)
            {
                return spawns.GetSpawnPosition();
            }
        }

        return Vector3.zero;
    }
}
