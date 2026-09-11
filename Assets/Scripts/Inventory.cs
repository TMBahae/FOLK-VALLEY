using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 5;
    [SerializeField] private NotificationSystem notificationSystem;
    [SerializeField] private int totalTrashNeeded = 28;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private CinematicSystem cinematicSystem;
    [SerializeField] private int secondMissionTotal = 14;
    
    private List<PickupItem> items = new List<PickupItem>();
    private int totalTrashDumped = 0;
    private bool missionComplete = false;
    private bool secondMissionActive = false;
    private int secondMissionProgress = 0;
    private bool keyFound = false;
    
    public int CurrentCount => items.Count;
    public bool IsFull => items.Count >= maxCapacity;
    public bool IsEmpty => items.Count == 0;
    public int TotalTrashDumped => totalTrashDumped;
    
    public bool AddItem(PickupItem item)
    {
        if (IsFull)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Inventory is full!", "#e02a0d");
            }
            return false;
        }
        
        items.Add(item);
        
        if (notificationSystem != null)
        {
            notificationSystem.ShowNotification("+1 " + item.itemName + " (" + items.Count + "/" + maxCapacity + ")");
        }
        
        return true;
    }
    
    public void ClearAll()
    {
        int count = items.Count;
        items.Clear();
        
        if (count == 0)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Inventory is empty!", "#e02a0d");
            }
            return;
        }
        
        if (!missionComplete)
        {
            totalTrashDumped += count;
            
            if (totalTrashDumped >= totalTrashNeeded)
            {
                missionComplete = true;
                secondMissionActive = true;
                
                if (notificationSystem != null)
                {
                    notificationSystem.ShowNotification("All trash was dumped!", "#b1fc03");
                }
                
                if (objectToEnable != null)
                {
                    objectToEnable.SetActive(true);
                }
                
                if (cinematicSystem != null)
                {
                    cinematicSystem.PlayCinematic(1);
                }
                
                totalTrashDumped = 0;
            }
            else
            {
                if (notificationSystem != null)
                {
                    notificationSystem.ShowNotification("Dumped " + count + " items. (" + totalTrashDumped + "/" + totalTrashNeeded + ")");
                }
            }
        }
        else if (secondMissionActive && !keyFound)
        {
            secondMissionProgress += count;
            
            if (secondMissionProgress >= secondMissionTotal)
            {
                keyFound = true;
                
                KeyManager keyManager = FindObjectOfType<KeyManager>();
                if (keyManager != null)
                {
                    keyManager.FoundKeys();
                }
                
                if (cinematicSystem != null)
                {
                    cinematicSystem.PlayCinematic(3);
                }
            }
            else
            {
                if (notificationSystem != null)
                {
                    notificationSystem.ShowNotification("Dumped " + count + " items. (" + secondMissionProgress + "/" + secondMissionTotal + ")");
                }
            }
        }
        else
        {
            if (notificationSystem != null && count > 0)
            {
                notificationSystem.ShowNotification("Dumped " + count + " items");
            }
        }
    }
    
    public void ResetMission()
    {
        missionComplete = false;
        totalTrashDumped = 0;
        secondMissionActive = false;
        secondMissionProgress = 0;
        keyFound = false;
    }
}