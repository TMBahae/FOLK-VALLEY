using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TriggerAudioFader : MonoBehaviour
{
    public string playerTag = "Player";
    public float insideVolume = 0.1f;
    public float fadeSpeed = 1.5f;

    private AudioSource audioSource;
    private float initialVolume;
    private float targetVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialVolume = audioSource.volume;
        targetVolume = initialVolume;
    }

    void Update()
    {
        if (Mathf.Approximately(audioSource.volume, targetVolume)) return;

        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            targetVolume = insideVolume;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            targetVolume = initialVolume;
        }
    }
}