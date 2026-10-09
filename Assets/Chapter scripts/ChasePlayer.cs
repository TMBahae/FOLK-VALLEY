using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float delayBeforeChase = 5f;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject objectToEnableOnDeath;
    
    [SerializeField] private GameObject light1;
    [SerializeField] private GameObject light2;
    [SerializeField] private GameObject light3;
    [SerializeField] private GameObject objectWithOutline;
    
    private NavMeshAgent agent;
    private bool chaseStarted = false;
    private bool isDead = false;
    
    public bool IsDead => isDead;
    
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
    
    public void RestartChase()
    {
        if (isDead) return;
        chaseStarted = true;
        enabled = true;
        
        if (agent != null)
            agent.isStopped = false;
        
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }
    
    public void StopAgent()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }
    
    public void ResetAgentPosition(Vector3 position)
    {
        if (agent != null)
            agent.Warp(position);
    }
    
    private void BeginChase()
    {
        if (isDead) return;
        
        enabled = true;
        
        if (agent != null)
            agent.isStopped = false;
        
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
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
        
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
        
        enabled = false;
        
        if (objectToEnableOnDeath != null)
            objectToEnableOnDeath.SetActive(true);
        
        if (light1 != null)
            light1.SetActive(true);
        if (light2 != null)
            light2.SetActive(true);
        if (light3 != null)
            light3.SetActive(true);
        
        if (objectWithOutline != null)
        {
            Outline outline = objectWithOutline.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = true;
            
            AudioSource objAudio = objectWithOutline.GetComponent<AudioSource>();
            if (objAudio != null)
                objAudio.Play();
        }
        
        Invoke(nameof(DisableSphere), 10f);
    }
    
    private void DisableSphere()
    {
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (isDead) return;
        if (agent == null || player == null) return;
        
        agent.SetDestination(player.position);
    }
}