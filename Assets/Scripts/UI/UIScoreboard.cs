using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIScoreboard : NetworkBehaviour
{
    [SerializeField] private GameObject playerHolderPrefab;
    [SerializeField] private Image teamIcon;
    [SerializeField] private TMP_Text teamText;

    [SerializeField] private Transform ctTeamContainer, tTeamContainer;
    
    [SerializeField] private List<UIElementPlayerHolder> _ctSlots;
    [SerializeField] private List<UIElementPlayerHolder> _tSlots;
    
    public List<ScoreboardEntry> ScoreboardEntries { get; set; }

    private void Start()
    {
        InitializeSlots();
    }

    private void Update()
    {
        UpdateScoreboardUI();
    }

    private void InitializeSlots()
    {
        Debug.Log("Initializing Slots...");

        if (ctTeamContainer == null || tTeamContainer == null)
        {
            Debug.LogError("Team containers are not assigned!");
            return;
        }

        Debug.Log("Slots initialized successfully.");
    }

    private void UpdateScoreboardUI()
    {
        foreach (UIElementPlayerHolder slot in _ctSlots.Concat(_tSlots))
        {
            slot.ClearInfo();
        }

        ScoreboardManager.Instance.GetScoreboardServer(this);
        if (ScoreboardEntries == null) return;
        
        foreach (var entry in ScoreboardEntries)
        {
            UIElementPlayerHolder slot = GetAvailableSlot(entry.PlayerTeam);
            if (slot != null)
            {
                slot.SetPlayerInfo(entry.PlayerName, entry.PlayerMoney, entry.PlayerKills, entry.PlayerDeaths, entry.KdRatio);
            }
        }
    }

    private UIElementPlayerHolder GetAvailableSlot(Player.PlayerTeams team)
    {
        List<UIElementPlayerHolder> slots = team == Player.PlayerTeams.CT ? _ctSlots : _tSlots;

        if (slots == null)
        {
            Debug.LogError("Slots list is null!");
            return null;
        }

        foreach (var slot in slots)
        {
            if (!slot.IsOccupied())
            {
                return slot;
            }
        }

        return null;
    }
}
