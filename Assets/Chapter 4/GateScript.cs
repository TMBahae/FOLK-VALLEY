using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GateScript : Interactable
{
    [SerializeField] private Animator doorAnim;
    public bool canBeDestroyed = false;
    private int hitCount = 0;
    [SerializeField] private AudioClip openCloseSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private CinematicSystem cS;
    [SerializeField] private GameObject longMan;
    [SerializeField] private MonsterCameraEffect mCE;

    public void OpenDaNoor()
    {
        doorAnim.SetTrigger("DoorOpen");
        GetComponent<AudioSource>().generator = openCloseSound;
        GetComponent<AudioSource>().Play();
    }

    public void CloseDaNoor()
    {
        doorAnim.SetTrigger("DoorClose");
        GetComponent<AudioSource>().generator = openCloseSound;
        GetComponent<AudioSource>().Play();
    }

    public override void Interact()
    {

        if(canBeDestroyed == true)
        {
            hitCount++;
            GetComponent<AudioSource>().generator = hitSound;
            GetComponent<AudioSource>().Play();
        }

        if(hitCount > 5)
        {
            longMan.SetActive(true);
            canBeDestroyed = false;
            OpenDaNoor();
            cS.PlayCinematic(10);
            mCE.PlayEffect();
        }
    }
}
