using UnityEngine;

public class TPRecorder : Interactable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private CinematicSystem cinematicSystem;
    [SerializeField] private int cinematicIndex = 0;
    
    [Header("Teleport Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform recorderTransform;
    [SerializeField] private float teleportDelay = 5f;
    [SerializeField] private float teleportOffsetX = 400f;
    
    [Header("Post-Teleport")]
    [SerializeField] private GameObject particleSystemToDisable;
    [SerializeField] private bool disableFog = true;
    [SerializeField] private ChasePlayer chasePlayer;
    
    private bool hasInteracted = false;
    
    public override void Interact()
    {
        if (hasInteracted) return;
        hasInteracted = true;
        
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        
        if (cinematicSystem != null)
        {
            cinematicSystem.PlayCinematic(cinematicIndex);
        }
        
        Invoke(nameof(TeleportNow), teleportDelay);
    }
    
    private void TeleportNow()
    {
        if (playerTransform != null)
        {
            Vector3 playerPos = playerTransform.position;
            playerPos.x += teleportOffsetX;
            playerTransform.position = playerPos;
        }
        
        if (recorderTransform != null)
        {
            Vector3 recorderPos = recorderTransform.position;
            recorderPos.x += teleportOffsetX;
            recorderTransform.position = recorderPos;
        }
        
        if (disableFog)
        {
            RenderSettings.fog = false;
        }
        
        if (particleSystemToDisable != null)
        {
            particleSystemToDisable.SetActive(false);
        }
        
        if (chasePlayer != null)
        {
            chasePlayer.StartChase();
        }
    }
}