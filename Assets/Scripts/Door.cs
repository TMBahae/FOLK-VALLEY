using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private NotificationSystem notificationSystem;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 2f;
    
    private bool isOpening = false;
    private bool isOpen = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    
    private void Start()
    {
        startRotation = transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0f, rotationAngle, 0f);
    }
    
    public override void Interact()
    {
        // Check if recording was listened to
        if (!AudioRecorder.HasListenedToRecording)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Find the recording first!", "#e02a0d");
            }
            return;
        }
        
        // Check if door is already open or opening
        if (isOpen || isOpening) return;
        
        // Check if keys were found
        if (!KeyManager.KeysFound)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Keys required to unlock!", "#e02a0d");
            }
            return;
        }
        
        // Open the door
        StartCoroutine(OpenDoor());
    }
    
    private System.Collections.IEnumerator OpenDoor()
    {
        isOpening = true;
        
        // Play sound
        if (audioSource != null && doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }
        
        // Rotate smoothly
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * rotationSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, timer);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, eased);
            yield return null;
        }
        
        transform.rotation = targetRotation;
        isOpen = true;
        isOpening = false;
        
        if (notificationSystem != null)
        {
            notificationSystem.ShowNotification("Door opened!", "#b1fc03");
        }
    }
}