using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls PlayerControls { get; private set; }
    public UI UI { get; private set; }

    private void Awake()
    {
        if (PlayerControls == null)
        {
            PlayerControls = new PlayerControls();
        }

        if (UI == null)
        {
            UI = new UI();
        }
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
        UI.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
        UI.Disable();
    }
}