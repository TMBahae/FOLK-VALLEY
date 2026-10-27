using System.Collections;
using UnityEngine;

public class IsInRoomTrigger : MonoBehaviour
{
    [SerializeField] private GateScript gateScript;
    [SerializeField] private AudioSource securityCamAudioSource;
    [SerializeField] private AudioClip helloAlex;
    [SerializeField] private AudioClip pleaseWait;
    [SerializeField] private CinematicSystem cS;
    [SerializeField] private Outline outLineScript;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;
            gateScript.CloseDaNoor();
            securityCamAudioSource.generator = helloAlex;
            securityCamAudioSource.Play();
            StartCoroutine(WaitAndPlayAlexSound());
        }
    }

    private IEnumerator WaitAndPlayAlexSound()
    {
        yield return new WaitForSeconds(21);
        cS.PlayCinematic(9);
        yield return new WaitForSeconds(4);
        securityCamAudioSource.generator = pleaseWait;
        securityCamAudioSource.Play();
        gateScript.canBeDestroyed = true;
        outLineScript.enabled = true;
    }
}
