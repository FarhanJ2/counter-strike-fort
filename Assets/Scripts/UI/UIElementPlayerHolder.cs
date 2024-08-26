using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElementPlayerHolder : MonoBehaviour
{
    [SerializeField] private Image _playerIcon;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerMoney;
    [SerializeField] private TMP_Text _playerKills;
    [SerializeField] private TMP_Text _playerDeaths;
    [SerializeField] private TMP_Text _playerKd;

    private bool isOccupied = false;

    public void SetPlayerInfo(string playerName, int playerMoney, int playerKills, int playerDeaths, float playerKd, Sprite playerIcon = null)
    {
        _playerName.text = playerName;
        _playerMoney.text = $"${playerMoney}";
        _playerKills.text = playerKills.ToString();
        _playerDeaths.text = playerDeaths.ToString();
        _playerKd.text = playerKd.ToString("F2");

        if (playerIcon != null)
        {
            _playerIcon.sprite = playerIcon;
            _playerIcon.enabled = true;
        }
        else
        {
            _playerIcon.enabled = false;
        }

        isOccupied = true;
        gameObject.SetActive(true);
    }

    public void ClearInfo()
    {
        _playerName.text = "";
        _playerMoney.text = "";
        _playerKills.text = "";
        _playerDeaths.text = "";
        _playerKd.text = "";
        _playerIcon.sprite = null;
        _playerIcon.enabled = false;

        isOccupied = false;
        gameObject.SetActive(false);
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }
}