using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 0f;
    
    [Header("Look")]
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float minLookAngle = -90f;
    [SerializeField] private float maxLookAngle = 90f;
    [SerializeField] private Transform cameraTransform;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip sprintSound;
    
    private PlayerControls playerControls;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private CharacterController characterController;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private Vector3 lastPosition;
    
    private bool canMove = true;
    private bool canLook = true;
    
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    
    public bool CanLook
    {
        get { return canLook; }
        set { canLook = value; }
    }

    public float GetVerticalRotation()
    {
        return verticalRotation;
    }
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        lastPosition = transform.position;
    }
    
    private void OnEnable()
    {
        moveAction = playerControls.Gameplay.Move;
        lookAction = playerControls.Gameplay.Look;
        jumpAction = playerControls.Gameplay.Jump;
        sprintAction = playerControls.Gameplay.Sprint;
        
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerSceneData.HasData)
        {
            characterController.enabled = false;
            transform.position = PlayerSceneData.Position;
            transform.rotation = PlayerSceneData.Rotation;
            
            verticalRotation = PlayerSceneData.CameraPitch;
            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            }
            
            characterController.enabled = true;
            PlayerSceneData.HasData = false;
        }
    }
    
    private void Update()
    {
        if (canLook)
        {
            HandleRotation();
        }
        
        if (canMove && characterController != null && characterController.enabled)
        {
            HandleMovement();
            ApplyGravity();
            HandleJump();
            HandleAudio();
        }
    }
    
    private void HandleRotation()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
        
        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
    
    private void HandleMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        
        float currentSpeed = sprintAction.IsPressed() ? sprintSpeed : walkSpeed;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }
    
    private void HandleJump()
    {
        if (jumpAction.WasPressedThisFrame() && IsGrounded() && jumpHeight > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    private void ApplyGravity()
    {
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleAudio()
    {
        Vector3 currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        currentVelocity.y = 0f;
        float currentSpeed = currentVelocity.magnitude;
        
        bool isMoving = currentSpeed > 0.1f && IsGrounded();
        
        if (isMoving)
        {
            bool isSprinting = currentSpeed > walkSpeed;
            AudioClip clip = isSprinting ? sprintSound : walkSound;
            
            if (audioSource.clip != clip)
            {
                audioSource.clip = clip;
            }
            
            if (!audioSource.isPlaying && clip != null)
            {
                audioSource.loop = true;
                
                if (isSprinting)
                {
                    audioSource.pitch = 1.3f;
                    audioSource.volume = 0.8f;
                }
                else
                {
                    audioSource.pitch = 1f;
                    audioSource.volume = 0.5f;
                }
                
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
    
    private bool IsGrounded()
    {
        float groundCheckDistance = 0.1f;
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + characterController.center;
        float rayLength = characterController.height / 2f + groundCheckDistance;
        
        return Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength);
    }
}