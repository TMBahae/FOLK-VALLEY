using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private NotificationSystem notificationSystem;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float crosshairScaleMultiplier = 5f;
    [SerializeField] private float crosshairScaleSpeed = 10f;
    
    private PlayerControls playerControls;
    private InputAction interactAction;
    private Interactable currentTarget;
    private Outline currentOutline;
    private Vector3 crosshairOriginalScale;
    private float crosshairCurrentScale = 1f;
    private bool isHovering = false;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        if (crosshair != null)
        {
            crosshairOriginalScale = crosshair.localScale;
        }
    }
    
    private void OnEnable()
    {
        interactAction = playerControls.Gameplay.Interact;
        interactAction.Enable();
        interactAction.performed += OnInteract;
    }
    
    private void OnDisable()
    {
        interactAction.performed -= OnInteract;
        interactAction.Disable();
        DisableOutline();
    }
    
    private void Update()
    {
        CheckForInteractable();
        HandleCrosshairScale();
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentTarget == null) return;
        
        if (currentTarget is AudioRecorder || currentTarget is BroomScript) // ADDED THIS FOR CHAPTER 2 | Luke
        {
            currentTarget.Interact();
            return;
        }
        
        if(FindAnyObjectByType<AudioRecorder>() != null) // Changed sth here for chapter 2 | Luke
        {
            if (!AudioRecorder.HasListenedToRecording)
            {
                if (notificationSystem != null)
                {
                    notificationSystem.ShowNotification("Find the recording first!");
                }
                return;
            }
        }

        if (FindAnyObjectByType<BroomScript>() != null) // Changed sth here for chapter 2 | Luke
        {
            if (!BroomScript.HasPickedUpBroom)
            {
                if (notificationSystem != null)
                {
                    notificationSystem.ShowNotification("Find the broom first!");
                }
                return;
            }
        }

        currentTarget.Interact();
    }
    
    private void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            
            if (interactable != null)
            {
                if (currentTarget != interactable)
                {
                    DisableOutline();
                    currentTarget = interactable;
                    EnableOutline(currentTarget.gameObject);
                }
                isHovering = true;
                return;
            }
        }
        
        DisableOutline();
        currentTarget = null;
        isHovering = false;
    }
    
    private void EnableOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            currentOutline = outline;
            currentOutline.enabled = true;
        }
    }
    
    private void DisableOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
    
    private void HandleCrosshairScale()
    {
        if (crosshair == null) return;
        
        float targetScale = isHovering ? crosshairScaleMultiplier : 1f;
        crosshairCurrentScale = Mathf.Lerp(crosshairCurrentScale, targetScale, Time.deltaTime * crosshairScaleSpeed);
        
        crosshair.localScale = crosshairOriginalScale * crosshairCurrentScale;
    }
}