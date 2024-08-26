using FishNet.Object;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
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

    public bool canMove = true;

    private Vector3 velocity;
    public bool isGrounded;
    private bool isSprinting;
    private bool isCrouching = false;
    private bool onLadder = false;

    float ladderGrabDistance = 1f;

    public float GetSpeed { get { return speed; } }
    public bool GetGrounded { get { return isGrounded; } }

    private void Awake() {
        controller = GetComponent<CharacterController>();
        
        oldSpeed = speed;

        InputManager.Instance.PlayerControls.Movement.Jump.started += _ => Jump();
        InputManager.Instance.PlayerControls.Movement.Prone.started += _ => Prone();
        InputManager.Instance.PlayerControls.Movement.Crouch.started += _ => Crouch();

        InputManager.Instance.PlayerControls.Movement.Run.started += _ =>
        {
            isSprinting = true;
        };

        InputManager.Instance.PlayerControls.Movement.Run.canceled += _ => 
        {
            isSprinting = false;
        };

    }

    public virtual void Update() {
        if (!IsOwner) return;
        if (canMove) {
            isGrounded = Physics.CheckSphere(groundCheckObject.transform.position, groundDistance, groundLayer);

            if (isGrounded && velocity.y < 0) {
                velocity.y = -2f;
            }
            
            float x = InputManager.Instance.PlayerControls.Movement.Strafe.ReadValue<float>();
            float z = InputManager.Instance.PlayerControls.Movement.Translation.ReadValue<float>();

            Vector3 move = transform.right * x + transform.forward * z;

            if (isGrounded)
            {
                if (isSprinting)
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
                speed = Mathf.Lerp(speed, oldSpeed, sprintChangingSpeed / 2 * Time.deltaTime);
            }


            controller.Move(move * speed * Time.deltaTime);

            velocity.y -= gravity * Time.deltaTime; // this might be the reason the player is going into the ground
            
            // Debug.Log("Current velocity" + velocity.y);

            controller.Move(velocity * Time.deltaTime);

            float fovEval = fovCurve.Evaluate(speed);
            mainCamera.fieldOfView = fovEval;
            weaponCamera.fieldOfView = fovEval;

            LadderLogic(move);  
        } 
    }

    private void Jump() {
        if (isGrounded) {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            speed = Mathf.Lerp(speed, speed - 2, sprintChangingSpeed * 4 * Time.deltaTime);
            airJumps = maxAirJumps;
        } else if (airJumps > 0) {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            airJumps--;
        }
    }

    private void Prone() {
        Debug.Log("Prone Start");
        // Prone Logic
    }

    private void Crouch() {
        isCrouching = !isCrouching;
        if (isCrouching) {
            controller.height = 0.779f;
            controller.center = new Vector3(0f, 0.57f, 0f);
        } else if (!isCrouching) {
            controller.height = 0.979f;
            controller.center = new Vector3(0f, 1f, 0f);
        }
    }

    
    private void LadderLogic(Vector3 move) {
        if (!onLadder) {
            // Not climbing a ladder
            if (Physics.Raycast(ladderRayCheck.transform.position, move, out RaycastHit raycastHit, ladderGrabDistance)) {
                // Debug.Log(raycastHit.transform);
                if (raycastHit.transform.TryGetComponent(out Ladder ladder)) {
                    GrabLadder();                
                }

                if (onLadder) {
                    // Debug.Log("Ladder Found");
                    // move.x = 0f;
                    // move.y = move.z;
                    // velocity = Vector3.zero;
                    velocity.y = Mathf.Sqrt(20);
                    isGrounded = true;
                }
            }
        } else {
            // Climbing the ladder
            if (Physics.Raycast(ladderRayCheck.transform.position, move, out RaycastHit raycastHit, ladderGrabDistance)) {
                // Debug.Log(raycastHit.transform);
                if (!raycastHit.transform.TryGetComponent(out Ladder ladder)) {
                    DropLadder();          
                } else {
                    DropLadder();
                }
            }   
        }
    }

    private void GrabLadder() {
        onLadder = true;
    }

    private void DropLadder() {
        onLadder = false;
    }
}