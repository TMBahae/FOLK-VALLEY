using System.Collections;
using UnityEngine;

public class FirstVoiceOverTriggerScript : MonoBehaviour
{
    [SerializeField] private CinematicSystem cS;
    [SerializeField] private GameObject firstAudioObject;
    private bool hasBeenActivated;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !hasBeenActivated)
        {
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        hasBeenActivated = true;
        firstAudioObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3);
        cS.PlayCinematic(5);
        Destroy(gameObject);
    }
}
