using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIScoreboard : MonoBehaviour
{
    [SerializeField] private GameObject playerHolderPrefab;
    [SerializeField] private Image teamIcon;
    [SerializeField] private TMP_Text teamText;

    [SerializeField] private Transform ctTeamContainer, tTeamContainer;

    private readonly List<UIElementPlayerHolder> _ctSlots = new List<UIElementPlayerHolder>();
    private readonly List<UIElementPlayerHolder> _tSlots = new List<UIElementPlayerHolder>();

    private void Start()
    {
        InitializeSlots();
        UpdateScoreboardUI();
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

        int maxPlayersPerTeam = 5;

        for (int i = 0; i < maxPlayersPerTeam; i++)
        {
            // CT Slots
            GameObject ctSlotObj = Instantiate(playerHolderPrefab, ctTeamContainer);
            UIElementPlayerHolder ctSlot = ctSlotObj.GetComponent<UIElementPlayerHolder>();
            if (ctSlot == null)
            {
                Debug.LogError("Failed to get UIElementPlayerHolder component from CT slot prefab.");
            }
            ctSlot.ClearInfo();
            _ctSlots.Add(ctSlot);

            // T Slots
            GameObject tSlotObj = Instantiate(playerHolderPrefab, tTeamContainer);
            UIElementPlayerHolder tSlot = tSlotObj.GetComponent<UIElementPlayerHolder>();
            if (tSlot == null)
            {
                Debug.LogError("Failed to get UIElementPlayerHolder component from T slot prefab.");
            }
            tSlot.ClearInfo();
            _tSlots.Add(tSlot);
        }

        Debug.Log("Slots initialized successfully.");
    }

    private void UpdateScoreboardUI()
    {
        foreach (UIElementPlayerHolder slot in _ctSlots.Concat(_tSlots))
        {
            slot.ClearInfo();
        }

        var scoreboardEntries = Scoreboard.Instance.GetScoreboard();

        foreach (var entry in scoreboardEntries)
        {
            UIElementPlayerHolder slot = GetAvailableSlot(entry.PlayerTeam);
            if (slot != null)
            {
                slot.SetPlayerInfo(entry.PlayerName, entry.PlayerMoney, entry.PlayerKills, entry.PlayerDeaths, entry.KDRatio);
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
