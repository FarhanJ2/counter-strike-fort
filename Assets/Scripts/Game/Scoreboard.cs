using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard Instance { get; private set; }

    private Player[] _players;

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
}