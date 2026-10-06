using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private float shootRange = 50f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    
    private Camera mainCamera;
    
    private void OnEnable()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }
    
    private void Shoot()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, shootRange))
        {
            ChasePlayer chase = hit.collider.GetComponent<ChasePlayer>();
            
            if (chase == null)
                chase = hit.collider.GetComponentInParent<ChasePlayer>();
            
            if (chase != null)
            {
                if (audioSource != null && shootSound != null)
                    audioSource.PlayOneShot(shootSound);
                
                chase.Die();
            }
        }
    }
}