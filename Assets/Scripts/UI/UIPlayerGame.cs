using System.ComponentModel.Design;
using UnityEngine;

public class UIPlayerGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject scoreBoardUI;
    [SerializeField] private GameObject teamSelectorUI;
    [SerializeField] private GameObject buyMenuUI;

    [SerializeField] private PlayerBridge _bridge;

    private void OnEnable()
    {
        InputManager.Instance.UI.UIPlayer.ToggleScoreboard.started += _ =>
        {
            scoreBoardUI.SetActive(true);
        };
        InputManager.Instance.UI.UIPlayer.ToggleScoreboard.canceled += _ =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            scoreBoardUI.SetActive(false);
        };
        InputManager.Instance.UI.UIPlayer.UIRightClick.started += _ =>
        {
            if (scoreBoardUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        };
        InputManager.Instance.UI.UIGlobal.ToggleTeamSelector.started += _ => ToggleTeamSelector();
        InputManager.Instance.UI.UIPlayer.ToggleBuyMenu.started += _ => ToggleBuyMenu();
    }

    public void ToggleTeamSelector()
    {
        _bridge.playerCamScript.mouseEnabled = buyMenuUI.activeSelf;
        teamSelectorUI.SetActive(!teamSelectorUI.activeSelf);
    }

    private void ToggleBuyMenu()
    {
        _bridge.playerCamScript.mouseEnabled = buyMenuUI.activeSelf;
        LockCursor(buyMenuUI.activeSelf);
        buyMenuUI.SetActive(!buyMenuUI.activeSelf);
    }

    private void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
