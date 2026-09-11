using UnityEngine;

public class StopAudioOnTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceToStop;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSourceToStop != null)
            {
                audioSourceToStop.Stop();
                Debug.Log("Audio stopped!");
            }
            else
            {
                // If no audio source assigned, try to find one on the object
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}