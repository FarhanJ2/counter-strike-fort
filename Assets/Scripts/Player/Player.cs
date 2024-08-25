using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerTeams
    {
        CT, T
    }
    
    public PlayerTeams PlayerTeam { get; private set; }
    public int playerHealth;

    [SerializeField] private int startingHealth = 100;

    private void Start()
    {
        playerHealth = startingHealth;
    }
}
