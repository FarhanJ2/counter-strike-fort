using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    [AllowMutableSyncType] private SyncVar<string> _playerName;
    
    public PlayerTeams PlayerTeam { get; private set; }
    public string PlayerName { get => _playerName.Value; private set => _playerName.Value = value; }
    public int PlayerMoney { get; set; }
    public int PlayerKills { get; private set; }
    public int PlayerDeaths { get; private set; }
    public int playerHealth;

    public PlayerWeapons ownedWeapons = new PlayerWeapons();
    
    public bool InBuyZone { get; set; }
    public bool InBombZone { get; set; }
    
    public enum PlayerTeams
    {
        CT, T, UNASSIGNED
    }
    
    [SerializeField] private UIPlayerGame playerUI;
    [SerializeField] private int startingHealth = 100;
    private PlayerBridge _bridge;

    // events
    public static event Action<Player> OnHealthChanged; // used to update amount of cts and ts alive
    
    private void Awake()
    {
        PlayerName = "Player " + Random.Range(0, 1000);
        _bridge = GetComponent<PlayerBridge>();
        OnCreateClient();
        PlayerMoney = 8000;
        PlayerTeam = PlayerTeams.UNASSIGNED;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
        {
            _bridge.playerCamera.enabled = false;
            _bridge.weaponCamera.enabled = false;
            _bridge.uiCamera.enabled = false;
            _bridge.uiHud.enabled = false;
            enabled = false;
        }
    }

    private void OnCreateClient() // when the player object is created not on SPAWN
    {
        PlayerTeam = PlayerTeams.UNASSIGNED;
        _bridge.playerMovement.canMove = false;
        playerHealth = startingHealth;
        playerUI.ToggleTeamSelector();
        
        foreach (GameObject model in _bridge.playerModels)
        {
            model.SetActive(false);
        }

        _bridge.cameraHolder.SetActive(false);
    }

    public void TakeDamage(int damage, string damageFrom)
    {
        playerHealth -= damage;
        OnHealthChanged?.Invoke(this);
        
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Die(damageFrom);
        }
    }

    private void Die(string deathReason)
    {
        // Instantiate() // drop weapon
        if (ownedWeapons.HasBomb)
        {
            _bridge.PlayerInventory.DropItem(Weapon.WeaponName.C4);
        }

        _bridge.playerMovement.canMove = false;
        _bridge.PlayerCam.mouseEnabled = false;
        _bridge.uiHud.SetDeathText("You died to " + deathReason + ".");
        _bridge.uiHud.ToggleDeathScreen(true);
        
        ownedWeapons.HasArmor = false;
        ownedWeapons.HasHelmet = false;
        ownedWeapons.CurrentPrimary = Weapon.WeaponName.NONE;
        ownedWeapons.CurrentSecondary = Weapon.WeaponName.NONE;
        
        PlayerDeaths++;
        GameManager.InvokeMajorEvent();
    }

    public void AssignTeam(int teamId)
    {
        AssignTeamServer(teamId);
        AssignTeamConfiguration(teamId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void AssignTeamServer(int teamId)
    {
        SetModelsObservers();
    }
    
    private void AssignTeamConfiguration(int teamId)
    {
        PlayerTeam = teamId == 0 ? PlayerTeams.CT : PlayerTeams.T; // ct 0, t 1 for entire program
        playerUI.ToggleTeamSelector();
        
        
        // tp player to a spawn
        // gameObject.transform.position = SpawnManager.Instance.GetFreeSpawn(PlayerTeam);
        SpawnManager.Instance.GetFreeSpawnServer(this); // this moves player to the spawn
        _bridge.playerMovement.canMove = true;
        
        _bridge.cameraHolder.SetActive(true);
        _bridge.playerMovement.canMove = true;
        _bridge.PlayerCam.mouseEnabled = true;
        
        OnHealthChanged?.Invoke(this);
    }
    
    [ObserversRpc(BufferLast = true)] // add buffer last so all late joining players can see
    private void SetModelsObservers()
    {
        foreach (GameObject model in _bridge.playerModels)
        {
            model.SetActive(false);
        }
        
        // document the player models later
        if (PlayerTeam == PlayerTeams.CT)
        {
            _bridge.playerModels[0].SetActive(true);
        }
        else
        {
            _bridge.playerModels[0].SetActive(true);
        }
    }
}

public class PlayerWeapons
{
    public bool HasArmor { get; set; }
    public bool HasHelmet { get; set; }
    public bool HasKit { get; set; }
    public bool HasBomb { get; set; }
    public Weapon.WeaponName CurrentPrimary { get; set; }
    public Weapon.WeaponName CurrentSecondary { get; set; }
}
