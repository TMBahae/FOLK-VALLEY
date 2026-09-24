using System.Collections;
using UnityEngine;

public class EnterPortalVoiceOver : MonoBehaviour
{
    [SerializeField] private CinematicSystem cS;

    public void OnEnterPortal()
    {
        StartCoroutine(StartVoiceOver());
    }

    IEnumerator StartVoiceOver()
    {
        yield return new WaitForSeconds(1.5f);
        cS.PlayCinematic(7);
    }
}
