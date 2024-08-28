using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

public class UIPlayerGame : MonoBehaviour
{
    [SerializeField] private PlayerBridge _bridge;
    
    [Header("UI Screens")]
    [SerializeField] private GameObject scoreBoardUI;
    [SerializeField] private GameObject teamSelectorUI;
    [SerializeField] private GameObject buyMenuUI;
    [SerializeField] private GameObject hudUI;

    private void Start()
    {
        if (_bridge == null)
        {
            Debug.LogError("Bridge is not assigned!");
            return;
        }

        if (_bridge.InputManager == null)
        {
            Debug.LogError("InputManager is not assigned!");
            return;
        }

        if (_bridge.InputManager.PlayerControls == null)
        {
            Debug.LogError("PlayerControls is not assigned");
        }

        if (_bridge.InputManager.UI == null)
        {
            Debug.LogError("UI is not assigned!");
            return;
        }
        
        _bridge.InputManager.UI.UIPlayer.ToggleScoreboard.started += _ =>
        {
            scoreBoardUI.SetActive(true);
        };
        _bridge.InputManager.UI.UIPlayer.ToggleScoreboard.canceled += _ =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            scoreBoardUI.SetActive(false);
        };
        _bridge.InputManager.UI.UIPlayer.UIRightClick.started += _ =>
        {
            if (scoreBoardUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        };
        _bridge.InputManager.UI.UIGlobal.ToggleTeamSelector.started += _ => ToggleTeamSelector();
        _bridge.InputManager.UI.UIPlayer.ToggleBuyMenu.started += _ => ToggleBuyMenu();
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
