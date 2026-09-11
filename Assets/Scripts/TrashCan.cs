using UnityEngine;

public class TrashCan : Interactable
{
    [SerializeField] private NotificationSystem notificationSystem;
    
    public override void Interact()
    {
        Inventory playerInventory = FindObjectOfType<Inventory>();
        
        if (playerInventory != null)
        {
            playerInventory.ClearAll();
        }
    }
}