using UnityEngine;

public class PlayAnimationSound: MonoBehaviour
{

    public void PlaySound(AudioClip sound)
    {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
