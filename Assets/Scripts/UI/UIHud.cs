using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHud : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _defuseKitText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private TMP_Text _playersAliveText;
    [SerializeField] private TMP_Text _roundTimerText;

    [SerializeField] private PlayerBridge _bridge;

    private void Update()
    {
        if (_bridge.player.PlayerTeam != Player.PlayerTeams.CT || !_bridge.player.ownedWeapons.HasKit)
        {
            _defuseKitText.text = "";
        } else if (_bridge.player.ownedWeapons.HasKit)
        {
            _defuseKitText.text = "You have a kit";
        }

        _healthText.text = _bridge.player.playerHealth.ToString();
        _moneyText.text = "$" + _bridge.player.PlayerMoney;

        _winsText.text = "Wins: CT: " + GameManager.Instance.CtWins + "T: " + GameManager.Instance.TWins;
        _playersAliveText.text =
            "CT: " + GameManager.Instance.CtPlayersAlive + "T: " + GameManager.Instance.TPlayersAlive;

        _roundTimerText.text = GameManager.Instance.GetTimerDisplay();
    }
}
