using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float bobVerticalAmount = 0.03f;
    [SerializeField] private float bobHorizontalAmount = 0.06f;
    [SerializeField] private float sprintMultiplier = 1.3f;
    [SerializeField] private Transform playerTransform;
    
    private float bobTimer = 0f;
    private Vector3 originalPos;
    private float currentSpeed = 0f;
    private Vector3 lastPosition;
    private bool wasMoving = false;
    private CharacterController playerController;
    
    private void Start()
    {
        originalPos = transform.localPosition;
        lastPosition = playerTransform.position;
        playerController = playerTransform.GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        Vector3 velocity = (playerTransform.position - lastPosition) / Time.deltaTime;
        lastPosition = playerTransform.position;
        velocity.y = 0f;
        currentSpeed = velocity.magnitude;
        
        bool isGrounded = playerController.isGrounded;
        bool isMoving = currentSpeed > 0.1f && isGrounded;
        
        if (isMoving)
        {
            float speedFactor = Mathf.Clamp01(currentSpeed / walkSpeed);
            float frequency = bobFrequency * speedFactor;
            
            if (currentSpeed > walkSpeed)
            {
                frequency *= sprintMultiplier;
            }
            
            bobTimer += Time.deltaTime * frequency;
            
            float verticalBob = Mathf.Sin(bobTimer * 2f) * bobVerticalAmount * speedFactor;
            float horizontalBob = Mathf.Sin(bobTimer) * bobHorizontalAmount * speedFactor;
            
            transform.localPosition = originalPos + new Vector3(horizontalBob, verticalBob, 0f);
            
            wasMoving = true;
        }
        else
        {
            if (wasMoving)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 10f);
                if (Vector3.Distance(transform.localPosition, originalPos) < 0.001f)
                {
                    wasMoving = false;
                    bobTimer = 0f;
                }
            }
        }
    }
}