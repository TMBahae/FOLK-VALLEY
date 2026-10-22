using UnityEngine;

public class OpenTheNoor : MonoBehaviour
{
    [SerializeField] Animator doorAnim;

    public void OpenDaNoor()
    {
        doorAnim.SetTrigger("DoorOpen");
    }
}
