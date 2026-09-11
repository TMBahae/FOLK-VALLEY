using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float minPauseDuration = 8f;
    public float maxPauseDuration = 20f;
    public float minFlickerSpeed = 0.04f;
    public float maxFlickerSpeed = 0.12f;

    private Light myLight;
    private float defaultIntensity;

    void Start()
    {
        myLight = GetComponent<Light>();
        defaultIntensity = myLight.intensity;
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPauseDuration, maxPauseDuration));

            int flickerCount = Random.Range(2, 4);

            for (int i = 0; i < flickerCount; i++)
            {
                myLight.intensity = 0f;
                yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));

                myLight.intensity = defaultIntensity;
                yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            }
        }
    }
}