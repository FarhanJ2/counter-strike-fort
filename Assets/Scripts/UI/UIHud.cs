using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class UIHud : NetworkBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TMP_Text deathText;
    
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _defuseKitText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private TMP_Text _playersAliveText;
    [SerializeField] private TMP_Text _roundTimerText;
    [SerializeField] private TMP_Text _xyVelocityText;

    [SerializeField] private PlayerBridge _bridge;

    public void ToggleHUD()
    {
        _winsText.enabled = !_winsText.enabled;
        _playersAliveText.enabled = !_playersAliveText.enabled;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
            enabled = false;
    }

    public void ToggleDeathScreen(bool show)
    {
        _healthText.enabled = show;
        _defuseKitText.enabled = show;
        _moneyText.enabled = show;
        _winsText.enabled = show;
        _playersAliveText.enabled = show;
        _roundTimerText.enabled = show;

        deathScreen.SetActive(show);
    }

    public void SetDeathText(string deathReason)
    {
        deathText.text = deathReason;
    }

    private void Update()
    {
        if (_bridge.player.PlayerTeam != Player.PlayerTeams.CT || !_bridge.player.ownedWeapons.HasKit)
        {
            _defuseKitText.text = "";
        }
        else if (_bridge.player.ownedWeapons.HasKit)
        {
            _defuseKitText.text = "You have a kit";
        }
        _healthText.text = _bridge.player.playerHealth.ToString();
        _moneyText.text = "$" + _bridge.player.PlayerMoney;

        _winsText.text = "Wins: CT: " + GameManager.Instance.CtWins + "T: " + GameManager.Instance.TWins;
        _playersAliveText.text =
            "CT: " + GameManager.Instance.CtPlayersAlive + "T: " + GameManager.Instance.TPlayersAlive;

        _roundTimerText.text = GameManager.Instance.GetTimerDisplay();
        _xyVelocityText.text = "Velocity: " + _bridge.player.playerMovement.GetXZVelocity();
    }
}