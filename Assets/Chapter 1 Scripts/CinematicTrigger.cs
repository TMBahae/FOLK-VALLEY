using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] private CinematicSystem cinematicSystem;
    [SerializeField] private int clipIndex = 0;
    [SerializeField] private bool playOnEnter = true;
    [SerializeField] private bool playOnExit = false;
    [SerializeField] private bool destroyAfterTrigger = true;
    
    private bool hasTriggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasTriggered) return;
        
        if (playOnEnter && cinematicSystem != null)
        {
            cinematicSystem.PlayCinematic(clipIndex);
            hasTriggered = true;
            
            if (destroyAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasTriggered) return;
        
        if (playOnExit && cinematicSystem != null)
        {
            cinematicSystem.PlayCinematic(clipIndex);
            hasTriggered = true;
            
            if (destroyAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}