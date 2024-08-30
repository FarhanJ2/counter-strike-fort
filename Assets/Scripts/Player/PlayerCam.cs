using FishNet.Object;
using UnityEngine;

public class PlayerCam : NetworkBehaviour
{
    [SerializeField] private PlayerBridge _bridge;
    private readonly float _cameraRestPosition = 0.8f;
    private readonly float _crouchCameraPosition = 0.6f;
    
    public bool mouseEnabled = true;

    public float mouseSens = 25f;
    public Transform body;
    private bool _isLeaning = false;
    private bool _isLeaningRight = false;
    private bool _isCrouching = false;
    public bool toggledLean = true;

    private float _xRot = 0;
    private float _cameraLean = 0;
    private float _headMove = 0;
    private float _crouchMove = 0;

    public float maxCameraLean = 30.0f; // adjust this value as needed for the lean effect


    // NETWORK VARS && OWNERSHIP FUNCTIONS
    [SerializeField] private Camera playerCamera, weaponCamera, uiCamera;
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Debug.Log("Player Cam Client Init");
        }
        else
        {
            // disables all other player controllers so the player doesnt control multiple players
            gameObject.GetComponent<PlayerCam>().enabled = false;
        }
    }
    
    private void Start()
    {
        if (playerCamera == null || weaponCamera == null || uiCamera == null) {
            Debug.LogError("Please Assign the Camera Values");
            return;
        }

        _bridge.InputManager.PlayerControls.Movement.LeanLeft.started += _ => {
            if (toggledLean) _isLeaning = !_isLeaning; // Toggle the leaning state
        };

        // MAKE A FUNCTION TO ALLOW TOGGLE LEAN
        // InputManager.Instance.playerControls.Movement.LeanLeft.canceled += _ => {
        //     // When the Q key is released, ensure that the leaning state is reset to false
        //     if (!toggledLean)
        //     {
        //         isLeaning = false;
        //     }
        // };

        _bridge.InputManager.PlayerControls.Movement.LeanRight.started += _ => {
            if (toggledLean) _isLeaningRight = !_isLeaningRight; // Toggle the leaning state
        };

        // FOR TOGGLE LEAN
        // InputManager.Instance.playerControls.Movement.LeanRight.canceled += _ => {
        //     // When the Q key is released, ensure that the leaning state is reset to false
        //     if (!toggledLean)
        //     {
        //         isLeaning = false;
        //     }
        // };

        _bridge.InputManager.PlayerControls.Movement.Crouch.started += _ => {
            _isCrouching = !_isCrouching;
        };
    }

    private void Update()
    {
        if (!base.IsOwner) {
            SetCameraEnabled(playerCamera, false);
            SetCameraEnabled(weaponCamera, false);
            SetCameraEnabled(uiCamera, false);
            return;
        }

        SetCameraEnabled(playerCamera, IsOwner);
        SetCameraEnabled(weaponCamera, IsOwner);
        SetCameraEnabled(uiCamera, IsOwner);

        if (mouseEnabled) {
            float mouseX = _bridge.InputManager.PlayerControls.Camera.MouseX.ReadValue<float>() * mouseSens * Time.deltaTime;
            float mouseY = _bridge.InputManager.PlayerControls.Camera.MouseY.ReadValue<float>() * mouseSens * Time.deltaTime;
            float lean = _bridge.InputManager.PlayerControls.Movement.Lean.ReadValue<float>();
            
            _xRot -= mouseY;
            _xRot = Mathf.Clamp(_xRot, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);

            // if (lean > 0)
            // {
            //     _cameraLean = Mathf.Lerp(_cameraLean, maxCameraLean, 0.01f);
            //     transform.localRotation *= Quaternion.Euler(0f, 0f, _cameraLean);
            //
            //     _headMove = Mathf.Lerp(_headMove, -1.5f, 0.01f);
            //     transform.localPosition = new Vector3(_headMove, _cameraRestPosition, 0.01f);
            // }
            
            if (_isLeaning) {
                _cameraLean = Mathf.Lerp(_cameraLean, maxCameraLean, 0.01f);
                transform.localRotation *= Quaternion.Euler(0f, 0f, _cameraLean);
            
                _headMove = Mathf.Lerp(_headMove, -1.5f, 0.01f);
                transform.localPosition = new Vector3(_headMove, _cameraRestPosition, 0.01f);
            } else if (!_isLeaning) {
                _cameraLean = Mathf.Lerp(_cameraLean, 0f, 0.008f);
                transform.localRotation *= Quaternion.Euler(0f, 0f, _cameraLean);
            
                _headMove = Mathf.Lerp(_headMove, 0f, 0.01f);
                transform.localPosition = new Vector3(_headMove, _cameraRestPosition, 0.01f);
            }
            
            if (_isLeaningRight) {
                _cameraLean = Mathf.Lerp(_cameraLean, -maxCameraLean, 0.01f);
                transform.localRotation *= Quaternion.Euler(0f, 0f, _cameraLean);
            
                _headMove = Mathf.Lerp(_headMove, 1.5f, 0.01f);
                transform.localPosition = new Vector3(_headMove, _cameraRestPosition, 0.01f);
            } else if (!_isLeaningRight) {
                _cameraLean = Mathf.Lerp(_cameraLean, 0f, 0.008f);
                transform.localRotation *= Quaternion.Euler(0f, 0f, _cameraLean);
            
                _headMove = Mathf.Lerp(_headMove, 0f, 0.01f);
                transform.localPosition = new Vector3(_headMove, _cameraRestPosition, 0.01f);
            }
             
            if (_isCrouching) {
                _crouchMove = Mathf.Lerp(_crouchMove, _crouchCameraPosition, 2f);
                transform.localPosition = new Vector3(0f, _crouchMove, 0.01f);
            } else if (!_isCrouching) {
                _crouchMove = Mathf.Lerp(_crouchMove, _cameraRestPosition, 2f);
                transform.localPosition = new Vector3(0f, _crouchMove, 0.01f);
            }
        }
    }

    private void SetCameraEnabled(Camera cam, bool isEnabled) {
        if (cam != null) {
            AudioListener audioListener = cam.GetComponent<AudioListener>();
            cam.enabled = isEnabled;
            if (audioListener != null) audioListener.enabled = isEnabled;
        } 
    }
}