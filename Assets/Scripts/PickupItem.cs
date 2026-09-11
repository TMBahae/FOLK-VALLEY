using UnityEngine;

public class PickupItem : Interactable
{
    public string itemName = "Item";
    
    public override void Interact()
    {
        Inventory playerInventory = FindObjectOfType<Inventory>();
        
        if (playerInventory != null)
        {
            bool added = playerInventory.AddItem(this);
            if (added)
            {
                gameObject.SetActive(false);
            }
        }
    }
}