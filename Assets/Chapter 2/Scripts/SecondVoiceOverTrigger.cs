using System.Collections;
using UnityEngine;

public class SecondVoiceOverTrigger : MonoBehaviour
{
    [SerializeField] private CinematicSystem cS;
    [SerializeField] private GameObject secondAudioObject;
    [SerializeField] private GameObject portal1;
    private bool hasBeenActivated;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenActivated)
        {
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        hasBeenActivated = true;
        portal1.SetActive(true);
        cS.PlayCinematic(6);
        yield return new WaitForSeconds(3);
        secondAudioObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
