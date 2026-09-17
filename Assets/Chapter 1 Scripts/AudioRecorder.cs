using UnityEngine;

public class AudioRecorder : Interactable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private NotificationSystem notificationSystem;
    
    private static bool hasListenedToRecording = false;
    public static bool HasListenedToRecording => hasListenedToRecording;
    
    private bool isPlaying = false;
    private bool hasPlayed = false;
    private bool isDepleted = false;
    
    public override void Interact()
    {
        // If depleted, show low battery message
        if (isDepleted)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Batteries are dead...", "#e02a0d");
            }
            return;
        }
        
        if (isPlaying)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Recording already playing");
            }
            return;
        }
        
        if (hasPlayed)
        {
            // After first use, mark as depleted
            isDepleted = true;
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Batteries died...", "#e02a0d");
            }
            return;
        }
        
        if (audioSource != null && audioClip != null)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Recording now playing");
            }
            
            audioSource.clip = audioClip;
            audioSource.Play();
            isPlaying = true;
            hasPlayed = true;
            hasListenedToRecording = true;
            
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("You can now interact with items!");
            }
            
            float clipLength = audioClip.length;
            Invoke(nameof(ResetPlayback), clipLength);
        }
    }
    
    private void ResetPlayback()
    {
        isPlaying = false;
    }
    
    public void ForceStop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        isPlaying = false;
    }
    
    // Reset for new game
    public void ResetRecorder()
    {
        isDepleted = false;
        hasPlayed = false;
        hasListenedToRecording = false;
        isPlaying = false;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}