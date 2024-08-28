using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player[] Players { get; private set; }

    public bool C4Planted { get; private set; }
    public bool C4Exploded { get; private set; }
    public int CtWins { get; private set; }
    public int TWins { get; private set; }
    public int TotalRounds { get; private set; }

    public int CtPlayersAlive { get; private set; }
    public int TPlayersAlive { get; private set; }

    public float freezeTime = 15f;
    public float roundTime = 120f;
    public float bombTime = 40f;
    private float _timeRemaining;
    private bool _timerRunning = false;

    private int _roundPhase = 0; // 0: Freeze Time, 1: Round Time, 2: Ended
    
    private static event Action OnMajorEvent; 
    
    /// <summary>
    /// Triggers an event when a significant game event occurs, such as a player death, bomb planted, or bomb exploded.
    /// </summary>
    public static void InvokeMajorEvent()
    {
        OnMajorEvent?.Invoke();
    }
    
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

        Player.OnHealthChanged += SetPlayerCounts;
        OnMajorEvent += CheckForRoundWin;
    }

    private void OnDisable()
    {
        Player.OnHealthChanged -= SetPlayerCounts;
    }

    public void StartRound()
    {
        _roundPhase = 1;
        _timeRemaining = roundTime;
        Debug.Log("Round time started. Time remaining: " + _timeRemaining);
        DisablePlayers(false);
    }

    private void FindAllPlayers()
    {
        Players = FindObjectsOfType<Player>();
    }

    public void CheckForRoundWin() // run on an event so everytime a player dies bomb planted explodes etc this checks not on update
    {
        if (CtPlayersAlive == 0) // CT players are all dead
        {
            TWins++;
            EndRound(); // End round
        }

        if (TPlayersAlive == 0) // T players are all dead
        {
            CtWins++;
            EndRound(); // End round
        }

        if (C4Planted && C4Exploded) // Bomb planted and exploded
        {
            TWins++;
            EndRound(); // End round
        }
    }

    private void EndRound()
    {
        _timerRunning = false;
        Debug.Log("Round ended");
        // Optionally trigger an event here to notify round end
    }

    private void RoundTimer()
    {
        _timeRemaining -= Time.deltaTime;
        Debug.Log("Time remaining: " + _timeRemaining);

        if (_timeRemaining <= 0)
        {
            _timeRemaining = 0;

            if (_roundPhase == 0)
            {
                StartRound();
            }
            else if (_roundPhase == 1)
            {
                EndRound();
            }
        }
    }

    private void DisablePlayers(bool freeze)
    {
        foreach (Player player in Players)
        {
            player.GetComponent<PlayerBridge>().playerMovement.canMove = !freeze;
        }
    }

    public void SetPlayerCounts(Player changedPlayer)
    {
        CtPlayersAlive = 0;
        TPlayersAlive = 0;

        foreach (Player player in Players)
        {
            if (player.PlayerTeam == Player.PlayerTeams.CT && player.playerHealth > 0)
            {
                CtPlayersAlive++;
            }
            else if (player.PlayerTeam == Player.PlayerTeams.T && player.playerHealth > 0)
            {
                TPlayersAlive++;
            }
        }

        if (_roundPhase == 0 && (CtPlayersAlive > 0 || TPlayersAlive > 0))
        {
            _timeRemaining = freezeTime;
            _timerRunning = true;
            DisablePlayers(true);
        }
    }

    public string GetTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Update()
    {
        if (_timerRunning)
        {
            RoundTimer();
        }

        FindAllPlayers();
    }
}