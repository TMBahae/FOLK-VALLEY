using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanableManager : MonoBehaviour
{
    [SerializeField] private int cleanableAmount;
    [SerializeField] private NotificationSystem nS;
    [SerializeField] private DoorScript door;
    [SerializeField] private GameObject broom;
    private int cleaned;

    public void OnClean()
    {
        cleaned++;
        nS.ShowNotification("Cleaned: " + cleaned.ToString() + "/" + cleanableAmount.ToString());
        if (cleaned >= cleanableAmount)
        {
            door.OpenDoor();
            Destroy(broom);
        }
    }
}
