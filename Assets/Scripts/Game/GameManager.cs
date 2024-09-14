using System;
using System.Collections;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [AllowMutableSyncType] private SyncVar<Player[]> _players;
    [AllowMutableSyncType] private SyncVar<bool> _c4Planted, _c4Exploded, _c4Defused;
    [AllowMutableSyncType] private SyncVar<int> _ctWins, _tWins;
    [AllowMutableSyncType] private SyncVar<int> _totalRounds;
    [AllowMutableSyncType] private SyncVar<int> _ctPlayersAlive, _tPlayersAlive;
    [AllowMutableSyncType] private SyncVar<int> _roundPhase; // 0: Freeze Time, 1: Round Time, 2: Ended
    [AllowMutableSyncType] private SyncVar<float> _timeRemaining;
    [AllowMutableSyncType] private SyncVar<bool> _timerRunning;
    
    public Player[] Players { get => _players.Value; private set => _players.Value = value; }

    public bool C4Planted { get => _c4Planted.Value; set => _c4Planted.Value = value; }
    public bool C4Exploded { get => _c4Exploded.Value; set => _c4Exploded.Value = value; }
    public bool C4Defused { get => _c4Defused.Value; set => _c4Defused.Value = value; }
    public int CtWins { get => _ctWins.Value; private set => _ctWins.Value = value; }
    public int TWins { get => _tWins.Value; private set => _tWins.Value = value; }
    public int TotalRounds { get => _totalRounds.Value; private set => _totalRounds.Value = value; }

    public int CtPlayersAlive { get => _ctPlayersAlive.Value; private set => _ctPlayersAlive.Value = value; }
    public int TPlayersAlive { get => _tPlayersAlive.Value; private set => _tPlayersAlive.Value = value; }
    public int RoundPhase { get => _roundPhase.Value; private set => _roundPhase.Value = value; }
    public float TimeRemaining { get => _timeRemaining.Value; private set => _timeRemaining.Value = value; }
    public bool TimerRunning {get => _timerRunning.Value; private set => _timerRunning.Value = value; }

    public float freezeTime = 15f;
    public float roundTime = 10f;
    public float afterRoundEnd = 5f;
    
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
            // DontDestroyOnLoad(gameObject);
        }

        Player.OnHealthChanged += SetPlayerCounts;
        OnMajorEvent += CheckForRoundWin;

        CtWins = 0;
        TWins = 0;
        RoundPhase = 0;
        TimerRunning = false;
        C4Planted = false;
        C4Exploded = false;
        C4Defused = false;
    }

    // private void Start()
    // {
    //     if (!base.IsServerInitialized)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void OnDisable()
    {
        Player.OnHealthChanged -= SetPlayerCounts;
    }

    private bool startRoundRan = false;
    
    [ObserversRpc]
    public void StartRound()
    {
        if (startRoundRan) return;
        
        startRoundRan = true;
        Debug.Log("Start round");
        AudioManager.Instance.PlaySound(Random.Range(0, 2) > .5
            ? AudioManager.VO.LETS_GO_CT_0
            : AudioManager.VO.LETS_GO_CT_1);
        RoundPhase = 1;
        TimeRemaining = roundTime;
        Debug.Log("Round time started. Time remaining: " + TimeRemaining);
        DisablePlayers(false);
    }

    private void FindAllPlayers()
    {
        Players = FindObjectsOfType<Player>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckForRoundWin() // run on an event so everytime a player dies bomb planted explodes etc this checks not on update
    {
        if (CtPlayersAlive == 0 && TPlayersAlive == 0 && !C4Planted) // draw the round if only one player is alive in the entire match
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.ROUND_DRAW);
            EndRound();
            return;
        }
        
        if (CtPlayersAlive == 0) // CT players are all dead
        {
            TWins++;
            AudioManager.Instance.PlaySound(AudioManager.Sound.T_WIN);
            EndRound(); // End round
            return;
        }

        if (TPlayersAlive == 0) // T players are all dead
        {
            CtWins++;
            AudioManager.Instance.PlaySound(AudioManager.Sound.CT_WIN);
            EndRound(); // End round
            return;
        }

        if (C4Planted && C4Exploded) // Bomb planted and exploded
        {
            TWins++;
            AudioManager.Instance.PlaySound(AudioManager.Sound.T_WIN);
            EndRound(); // End round
            return;
        }

        if (C4Planted && C4Defused)
        {
            CtWins++;
            AudioManager.Instance.PlaySound(AudioManager.Sound.BOMB_DEF);
            StartCoroutine(C4DefusedSound());
            EndRound();
            return;
        }
    }

    IEnumerator C4DefusedSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.BOMB_DEF);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.PlaySound(AudioManager.Sound.CT_WIN);
    }
    
    private bool endRoundRan = false;
    
    [ObserversRpc]
    private void EndRound()
    {
        if (endRoundRan) return;

        endRoundRan = true;
        // AudioManager.Instance.PlaySound(AudioManager.Sound.CT_WIN);
        TimerRunning = false;
        Debug.Log("Round ended");
    }
    
    private void RoundTimer()
    {
        // Debug.Log("Timer Running: " + TimerRunning + ", TimeRemaining: " + TimeRemaining);
        TimeRemaining -= Time.deltaTime;
        // Debug.Log("Time after decrement: " + TimeRemaining);
        if (TimeRemaining <= 0)
        {
            TimeRemaining = 0;

            if (RoundPhase == 0)
            {
                StartRound();
            }
            else if (RoundPhase == 1) // ct win by time ending
            {
                EndRound();
                
            }
        }
    }
    
    [ObserversRpc]
    private void DisablePlayers(bool freeze)
    {
        foreach (Player player in Players)
        {
            player.GetComponent<PlayerBridge>().playerMovement.canMove = !freeze;
        }
    }
    
    [ServerRpc(RequireOwnership = false)] 
    public void SetPlayerCounts(Player changedPlayer)
    {
        Debug.Log("Setting the player counts");
        
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

        if (RoundPhase == 0 && (CtPlayersAlive > 0 || TPlayersAlive > 0))
        {
            TimeRemaining = freezeTime;
            TimerRunning = true;
            DisablePlayers(true);
        }
    }

    private bool startRan = false;
    private void CheckPlayerCounts()
    {
        if (Players == null || Players.Length == 0)
        {
            return;
        }

        int ctAliveCount = 0;
        int tAliveCount = 0;

        foreach (var player in Players)
        {
            if (player.PlayerTeam == Player.PlayerTeams.CT && player.playerHealth > 0)
            {
                ctAliveCount++;
            }
            else if (player.PlayerTeam == Player.PlayerTeams.T && player.playerHealth > 0)
            {
                tAliveCount++;
            }
        }

        // Update the player counts
        CtPlayersAlive = ctAliveCount;
        TPlayersAlive = tAliveCount;
        
        if (RoundPhase == 0 && (CtPlayersAlive > 0 || TPlayersAlive > 0) && !startRan)
        {
            startRan = true;
            TimeRemaining = freezeTime;
            TimerRunning = true;
            DisablePlayers(true);
        }
        
        // Debug.Log($"CT Players Alive: {CtPlayersAlive}, T Players Alive: {TPlayersAlive}");
    }

    public string GetTimerDisplay()
    {
        if (C4Planted)
        {
            return "C4 Planted";
        }
        
        int minutes = Mathf.FloorToInt(TimeRemaining / 60);
        int seconds = Mathf.FloorToInt(TimeRemaining % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Update()
    {
        if (!base.IsServerInitialized) return;
        
        if (TimerRunning)
        {
            RoundTimer();
        }

        FindAllPlayers();
        CheckPlayerCounts();
        // CheckForRoundWin();
    }
}