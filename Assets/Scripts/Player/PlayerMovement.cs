using System.Collections;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private PlayerBridge _bridge;

    private CharacterController controller;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float runSpeed = 15f;
    [SerializeField] private float sprintChangingSpeed = 1.5f;
    [SerializeField] private AnimationCurve fovCurve;
    private float oldSpeed;


    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpForce = 200;
    [SerializeField] private int maxAirJumps = 1;
    private int airJumps;

    [SerializeField] private GameObject groundCheckObject;
    [SerializeField] private GameObject ladderRayCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera weaponCamera;

    public bool canMove = false;

    private Vector3 _velocity;
    public bool isGrounded;
    private bool _isSprinting;
    private bool _isCrouching = false;
    private bool _onLadder = false;

    float ladderGrabDistance = 1f;

    private string _floorSurface;
    private bool _canPlayFootstep = true;

    public float GetSpeed
    {
        get { return speed; }
    }

    public bool GetGrounded
    {
        get { return isGrounded; }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        oldSpeed = speed;

        _bridge.InputManager.PlayerControls.Movement.Jump.started += _ => Jump();
        _bridge.InputManager.PlayerControls.Movement.Prone.started += _ => Prone();
        _bridge.InputManager.PlayerControls.Movement.Crouch.started += _ => Crouch();

        _bridge.InputManager.PlayerControls.Movement.Run.started += _ => { _isSprinting = true; };

        _bridge.InputManager.PlayerControls.Movement.Run.canceled += _ => { _isSprinting = false; };
    }

    private float fallSpeed = 0f; // To track maximum fall speed
    public float fallDamageThreshold = 10f; // Minimum speed to take fall damage
    public float fallDamageMultiplier = 2f; // Damage multiplier based on fall speed


    private bool played = false;
    public virtual void Update()
    {
        if (!IsOwner) return;
        if (canMove)
        {
            isGrounded = Physics.CheckSphere(groundCheckObject.transform.position, groundDistance, groundLayer);

            if (Physics.Raycast(groundCheckObject.transform.position, Vector3.down, out RaycastHit hit,
                    groundDistance + 1f, groundLayer))
            {
                string floorTag = hit.collider.gameObject.tag;
                _floorSurface = floorTag;
            }

            CalculateFallDamage();

            float x = _bridge.InputManager.PlayerControls.Movement.Strafe.ReadValue<float>();
            float z = _bridge.InputManager.PlayerControls.Movement.Translation.ReadValue<float>();

            Vector3 move = transform.right * x + transform.forward * z;

            if (isGrounded)
            {
                if (_isSprinting)
                {
                    speed = Mathf.Lerp(speed, runSpeed, sprintChangingSpeed * Time.deltaTime);
                }
                else
                {
                    speed = Mathf.Lerp(speed, oldSpeed, sprintChangingSpeed * Time.deltaTime);
                }
            }
            else
            {
                speed = Mathf.Lerp(speed, oldSpeed * 0.1f, sprintChangingSpeed * Time.deltaTime);
            }

            controller.Move(move * speed * Time.deltaTime);
            _velocity.y -= gravity * Time.deltaTime;

            controller.Move(_velocity * Time.deltaTime);

            float fovEval = fovCurve.Evaluate(speed);
            mainCamera.fieldOfView = fovEval;
            weaponCamera.fieldOfView = fovEval;

            LadderLogic(move);
            HandleMovementSound(move);
        }
    }

    private void CalculateFallDamage()
    {
        if (!isGrounded) // to track falling speed
        {
            if (_velocity.y < 0) // only track negative (falling speed)
            {
                fallSpeed = Mathf.Min(fallSpeed, _velocity.y); // keep the lowest (most negative) y velocity
                if (!played && fallSpeed < -fallDamageThreshold)
                {
                    _bridge.playerSounds.PlayFallingSound();
                    played = true;
                }
            }
        }
        else if (isGrounded && _velocity.y < 0) // when player hits the ground
        {
            if (fallSpeed < -fallDamageThreshold) // check if its over the threshold
            {
                _bridge.playerSounds.StopSound();
                float fallDamage = Mathf.Abs(fallSpeed) * fallDamageMultiplier;
                Debug.Log("Player took fall damage: " + fallDamage);

                _bridge.player.TakeDamage((int)fallDamage, "fall damage");
            }
            
            if (jumped) // play landing sound even if player didnt receive fall damage
            {
                _bridge.playerSounds.PlayLandingSound();
                jumped = false; 
            }

            // reset values here
            fallSpeed = 0f;
            _velocity.y = -2f;
            played = false;
        }
    }

    private void HandleMovementSound(Vector3 move)
    {
        if (move.magnitude > 0 && isGrounded && _canPlayFootstep)
        {
            StartCoroutine(PlayFootstepSound());
        }
    }

    private IEnumerator PlayFootstepSound()
    {
        _canPlayFootstep = false;
        _bridge.playerSounds.PlayFootstep(_floorSurface);
        yield return new WaitForSeconds(_isSprinting ? .25f : .3f);
        _canPlayFootstep = true;
    }

    private bool jumped = false;
    private void Jump()
    {
        if (isGrounded)
        {
            _velocity.y = Mathf.Sqrt(Mathf.Abs(jumpForce * -2f * gravity));
            speed = Mathf.Lerp(speed, speed - 2, sprintChangingSpeed * 4 * Time.deltaTime);
            airJumps = maxAirJumps;
            jumped = true;
        }
        // else if (airJumps >= 0)
        // {
        //     _velocity.y = Mathf.Sqrt(Mathf.Abs(jumpForce * -2f * gravity));
        //     airJumps--;
        //     jumped = true;
        // }
    }

    private void Prone()
    {
        Debug.Log("Prone Start");
        // Prone Logic
    }

    private void Crouch()
    {
        _isCrouching = !_isCrouching;
        if (_isCrouching)
        {
            controller.height = 0.779f;
            controller.center = new Vector3(0f, 0.57f, 0f);
        }
        else if (!_isCrouching)
        {
            controller.height = 0.979f;
            controller.center = new Vector3(0f, 1f, 0f);
        }
    }


    private void LadderLogic(Vector3 move)
    {
        if (!_onLadder)
        {
            // Not climbing a ladder
            if (Physics.Raycast(ladderRayCheck.transform.position, move, out RaycastHit raycastHit, ladderGrabDistance))
            {
                // Debug.Log(raycastHit.transform);
                if (raycastHit.transform.TryGetComponent(out Ladder ladder))
                {
                    GrabLadder();
                }

                if (_onLadder)
                {
                    // Debug.Log("Ladder Found");
                    // move.x = 0f;
                    // move.y = move.z;
                    // velocity = Vector3.zero;
                    _velocity.y = Mathf.Sqrt(20);
                    isGrounded = true;
                }
            }
        }
        else
        {
            // Climbing the ladder
            if (Physics.Raycast(ladderRayCheck.transform.position, move, out RaycastHit raycastHit, ladderGrabDistance))
            {
                // Debug.Log(raycastHit.transform);
                if (!raycastHit.transform.TryGetComponent(out Ladder ladder))
                {
                    DropLadder();
                }
                else
                {
                    DropLadder();
                }
            }
        }
    }

    private void GrabLadder()
    {
        _onLadder = true;
    }

    private void DropLadder()
    {
        _onLadder = false;
    }
}