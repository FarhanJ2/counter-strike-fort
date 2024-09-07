using System.ComponentModel.Design;
using FishNet.Object;
using TMPro;
using UnityEngine;

public class UIPlayerGame : NetworkBehaviour
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
            _bridge.uiHud.ToggleHUD();
        };
        _bridge.InputManager.UI.UIPlayer.ToggleScoreboard.canceled += _ =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _bridge.uiHud.ToggleHUD();
            scoreBoardUI.SetActive(false);
            _bridge.PlayerCam.mouseEnabled = true;
        };
        _bridge.InputManager.UI.UIPlayer.UIRightClick.started += _ =>
        {
            if (scoreBoardUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _bridge.PlayerCam.mouseEnabled = false;
            }
        };
        _bridge.InputManager.UI.UIGlobal.ToggleTeamSelector.started += _ => ToggleTeamSelector();
        _bridge.InputManager.UI.UIPlayer.ToggleBuyMenu.started += _ => ToggleBuyMenu();

        Buyzone.OnExitBuyzone += ToggleBuyMenu;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            
        }
        else
        {
            scoreBoardUI.SetActive(false);
            teamSelectorUI.SetActive(false);
            buyMenuUI.SetActive(false);
            hudUI.SetActive(false);

            gameObject.GetComponent<UIPlayerGame>().enabled = false;
        }
    }

    public void ToggleTeamSelector()
    {
        _bridge.PlayerCam.mouseEnabled = buyMenuUI.activeSelf;
        teamSelectorUI.SetActive(!teamSelectorUI.activeSelf);
    }

    private void ToggleBuyMenu()
    {
        if (_bridge.player.InBuyZone)
        {
            bool isMenuActive = buyMenuUI.activeSelf;
            _bridge.PlayerCam.mouseEnabled = isMenuActive;
            LockCursor(isMenuActive);
            _bridge.uiHud.ToggleHUD();
            buyMenuUI.SetActive(!isMenuActive);
        }
        else
        {
            buyMenuUI.SetActive(false);
            _bridge.PlayerCam.mouseEnabled = true;
            LockCursor(true);
        }
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
