using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
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
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public Vector3 GetFreeSpawn(Player.PlayerTeams team)
    {
        foreach (Spawn spawns in team == Player.PlayerTeams.CT ? _ctSpawns : _tSpawns)
        {
            if (!spawns.IsOccupied)
            {
                return spawns.GetSpawnPosition();
            }
        }

        return new Vector3();
    }
}
