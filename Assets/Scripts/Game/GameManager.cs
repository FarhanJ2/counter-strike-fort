using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public float roundTime = 10f;
    public float bombTime = 40f;
    public float afterRoundEnd = 5f;
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

        CtWins = 0;
        TWins = 0;
    }

    private void OnDisable()
    {
        Player.OnHealthChanged -= SetPlayerCounts;
    }

    public void StartRound()
    {
        AudioManager.Instance.PlaySound(Random.Range(0, 1) > .5
            ? AudioManager.VO.LETS_GO_CT_0
            : AudioManager.VO.LETS_GO_CT_1);
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
            AudioManager.Instance.PlaySound(AudioManager.Sound.T_WIN);
            EndRound(); // End round
        }

        if (TPlayersAlive == 0) // T players are all dead
        {
            CtWins++;
            AudioManager.Instance.PlaySound(AudioManager.Sound.CT_WIN);
            EndRound(); // End round
        }

        if (C4Planted && C4Exploded) // Bomb planted and exploded
        {
            TWins++;
            AudioManager.Instance.PlaySound(AudioManager.Sound.T_WIN);
            EndRound(); // End round
        }
    }

    private void EndRound()
    {
        _timerRunning = false;
        Debug.Log("Round ended");
    }

    private void RoundTimer()
    {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0)
        {
            _timeRemaining = 0;

            if (_roundPhase == 0)
            {
                StartRound();
            }
            else if (_roundPhase == 1) // ct win by time ending
            {
                EndRound();
                AudioManager.Instance.PlaySound(AudioManager.Sound.CT_WIN);
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