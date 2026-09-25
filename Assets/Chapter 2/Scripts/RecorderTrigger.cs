using UnityEngine;

public class RecorderTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource recorderAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            recorderAudioSource.Play();
            Destroy(gameObject);
        }
    }
}
