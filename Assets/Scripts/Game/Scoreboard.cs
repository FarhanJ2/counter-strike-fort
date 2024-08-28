using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard Instance { get; private set; }
    
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

    [CanBeNull]
    private Player GetPlayer(string playerName)
    {
        foreach (Player player in GameManager.Instance.Players)
        {
            if (player.PlayerName == playerName)
            {
                return player;
            }
        }

        return null;
    }

    private Player.PlayerTeams? GetPlayerTeam(string playerName)
    {
        Player player = GetPlayer(playerName);
        if (player == null)
        {
            Debug.LogError("This player does not exist...");
            return null;
        }

        return player.PlayerTeam;
    }

    private float? GetKdOfPlayer(string playerName)
    {
        Player player = GetPlayer(playerName);
        
        if (player == null)
        {
            Debug.LogError("This player does not exist...");
            return null;
        }
        
        return player.PlayerKills / player.PlayerDeaths;
    }

    public List<ScoreboardEntry> GetScoreboard()
    {
        return GameManager.Instance.Players
            .Select(p => new ScoreboardEntry
            {
                PlayerName = p.PlayerName,
                PlayerKills = p.PlayerKills,
                PlayerDeaths = p.PlayerDeaths,
                PlayerMoney = p.PlayerMoney,
                KdRatio = p.PlayerDeaths == 0 ? p.PlayerKills : (float)p.PlayerKills / p.PlayerDeaths,
                PlayerTeam = p.PlayerTeam
            })
            .ToList();
    }
}

public class ScoreboardEntry
{
    public string PlayerName { get; set; }
    public int PlayerMoney { get; set; }
    public Player.PlayerTeams PlayerTeam { get; set; }
    public int PlayerKills { get; set; }
    public int PlayerDeaths { get; set; }
    public float KdRatio { get; set; }
}
