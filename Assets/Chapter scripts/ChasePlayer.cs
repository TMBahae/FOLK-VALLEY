using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float delayBeforeChase = 5f;
    
    private NavMeshAgent agent;
    private bool chaseStarted = false;
    private bool isDead = false;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        agent.speed = chaseSpeed;
        agent.isStopped = true;
        
        enabled = false;
    }
    
    public void StartChase()
    {
        if (chaseStarted || isDead) return;
        chaseStarted = true;
        
        Invoke(nameof(BeginChase), delayBeforeChase);
    }
    
    private void BeginChase()
    {
        if (isDead) return;
        
        enabled = true;
        
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
    
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        
        enabled = false;
    }
    
    private void Update()
    {
        if (isDead) return;
        if (agent == null || player == null) return;
        
        agent.SetDestination(player.position);
    }
}