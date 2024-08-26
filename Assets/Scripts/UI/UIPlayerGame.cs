using System.ComponentModel.Design;
using UnityEngine;

public class UIPlayerGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject scoreBoardUI;
    [SerializeField] private GameObject teamSelectorUI;

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
    }

    public void ToggleTeamSelector()
    {
        teamSelectorUI.SetActive(!teamSelectorUI.activeSelf);
    }
}
