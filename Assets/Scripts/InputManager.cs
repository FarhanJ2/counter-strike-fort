using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public PlayerControls PlayerControls;
    public UI UI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PlayerControls = new PlayerControls();
            UI = new UI();
        }
        else
        {
            Destroy(gameObject);
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