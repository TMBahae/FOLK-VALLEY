using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform sphereTransform;
    [SerializeField] private ChasePlayer chasePlayer;
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private CanvasGroup deathCanvasGroup;
    
    [Header("Respawn Positions")]
    [SerializeField] private Vector3 sphereRespawnPos = new Vector3(446f, 1.09f, -101f);
    [SerializeField] private Vector3 playerRespawnPos = new Vector3(446f, 0f, -95f);
    
    [Header("Gun Pieces")]
    [SerializeField] private GameObject[] gunPieces;
    
    [Header("Timings")]
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private float fadeInTime = 1.5f;
    [SerializeField] private float blackTime = 1f;
    [SerializeField] private float fadeOutTime = 1.5f;
    [SerializeField] private float restartDelay = 2f;
    
    private bool isDead = false;
    
    private void Start()
    {
        if (deathCanvasGroup != null)
        {
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (chasePlayer == null || chasePlayer.IsDead) return;
        if (!other.CompareTag("Player")) return;
        
        StartCoroutine(DeathSequence());
    }
    
    private IEnumerator DeathSequence()
    {
        isDead = true;
        
        chasePlayer.enabled = false;
        chasePlayer.StopAgent();
        
        if (playerController != null)
        {
            playerController.CanMove = false;
            playerController.CanLook = false;
        }
        
        yield return new WaitForSeconds(freezeTime);
        
        deathCanvasGroup.gameObject.SetActive(true);
        deathCanvasGroup.alpha = 0f;
        
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Clamp01(t / fadeInTime);
            yield return null;
        }
        deathCanvasGroup.alpha = 1f;
        
        yield return new WaitForSeconds(blackTime);
        
        if (sphereTransform != null)
            sphereTransform.position = sphereRespawnPos;
        if (playerTransform != null)
            playerTransform.position = playerRespawnPos;
        
        if (chasePlayer != null)
            chasePlayer.ResetAgentPosition(sphereRespawnPos);
        
        GunPiece.ResetPieces();
        
        if (gunPieces != null)
        {
            foreach (GameObject piece in gunPieces)
            {
                if (piece != null)
                    piece.SetActive(true);
            }
        }
        
        yield return new WaitForSeconds(0.2f);
        
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            deathCanvasGroup.alpha = 1f - Mathf.Clamp01(t / fadeOutTime);
            yield return null;
        }
        deathCanvasGroup.alpha = 0f;
        deathCanvasGroup.gameObject.SetActive(false);
        
        if (playerController != null)
        {
            playerController.CanMove = true;
            playerController.CanLook = true;
        }
        
        yield return new WaitForSeconds(restartDelay);
        
        if (!chasePlayer.IsDead)
        {
            chasePlayer.RestartChase();
        }
        
        isDead = false;
    }
}