using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public PlayerTeams PlayerTeam { get; private set; }
    public string PlayerName { get; private set; }
    public int PlayerMoney { get; private set; }
    public int PlayerKills { get; private set; }
    public int PlayerDeaths { get; private set; }
    public int playerHealth;
    
    public enum PlayerTeams
    {
        CT, T, UNASSIGNED
    }
    
    [SerializeField] private UIPlayerGame playerUI;
    [SerializeField] private int startingHealth = 100;
    private PlayerBridge _bridge;

    private void Awake()
    {
        PlayerName = "Player " + Random.Range(0, 1000);
        _bridge = GetComponent<PlayerBridge>();
        OnCreateClient();
    }

    private void OnCreateClient() // when the player object is created not on SPAWN
    {
        playerHealth = startingHealth;
        playerUI.ToggleTeamSelector();
        
        foreach (GameObject model in _bridge.playerModels)
        {
            model.SetActive(false);
        }

        _bridge.cameraHolder.SetActive(false);
    }

    private void OnPlayerDeath()
    {
        PlayerDeaths++;
    }

    public void AssignTeam(int teamId)
    {
        PlayerTeam = teamId == 0 ? PlayerTeams.CT : PlayerTeams.T;
        playerUI.ToggleTeamSelector();
    
        
        // document the player models later
        if (PlayerTeam == PlayerTeams.CT)
        {
            _bridge.playerModels[0].SetActive(true);
        }
        else
        {
            _bridge.playerModels[0].SetActive(true);
        }

        _bridge.cameraHolder.SetActive(true);
    }
}
