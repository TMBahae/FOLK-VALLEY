using UnityEngine;
using UnityEngine.AI;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private ChasePlayer chasePlayer;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip chaseSound;
    
    private bool isWalking = false;
    private bool wasChasing = false;
    
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        if (animator == null)
            animator = GetComponent<Animator>();
        
        animator.speed = 0f;
    }
    
    private void Update()
    {
        if (player == null) return;
        
        bool isChasing = chasePlayer != null && !chasePlayer.IsDead && IsChasing();
        
        if (isChasing)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        
        if (isChasing != isWalking)
        {
            isWalking = isChasing;
            animator.speed = isChasing ? 1f : 0f;
        }
        
        if (isChasing && !wasChasing)
        {
            if (audioSource != null && chaseSound != null)
                audioSource.Play();
        }
        else if (!isChasing && wasChasing)
        {
            if (audioSource != null)
                audioSource.Stop();
        }
        
        wasChasing = isChasing;
    }
    
    private bool IsChasing()
    {
        NavMeshAgent agent = chasePlayer.GetComponent<NavMeshAgent>();
        if (agent == null) return false;
        return agent.enabled && !agent.isStopped && agent.velocity.sqrMagnitude > 0.01f;
    }
}