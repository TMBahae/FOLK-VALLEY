using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public void OpenDoor()
    {
        GetComponent<Animator>().SetTrigger("OpenDoor");
        GetComponent<AudioSource>().Play();
    }
}
