using UnityEngine;

public class RotateAndPlaySound : MonoBehaviour
{
    public AudioClip sound;
    private float angle = 180f;
    private float timer = 0.2f;
    private bool started;

    void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
    }

    void Update()
    {
        if (!started)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                started = true;
                GetComponent<AudioSource>().PlayOneShot(sound);
            }
            return;
        }

        if (angle <= 90f) return;
        angle -= 180f * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }
}