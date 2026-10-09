using UnityEngine;

public class Cleanable : Interactable
{
    [SerializeField] CleanableManager manager;
    [SerializeField] NotificationSystem notificationSystem;

    public override void Interact()
    {
        if (BroomScript.HasPickedUpBroom)
        {
            manager.OnClean();
            Destroy(gameObject);
        }
        else
        {
            notificationSystem.ShowNotification("Find the Broom first!");
        }
    }
}
