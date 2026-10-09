using UnityEngine;

public class RecorderTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource recorderAudioSource;
[SerializeField] public GameObject CineTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
if (CineTrigger != null)
            CineTrigger.SetActive(true);
            recorderAudioSource.Play();
            Destroy(gameObject);

        }
    }
}
