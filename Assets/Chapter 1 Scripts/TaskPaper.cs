using UnityEngine;

public class TaskPaper : Interactable
{
    [SerializeField] private TaskList taskList;
    [SerializeField] private NotificationSystem notificationSystem;
    
    public override void Interact()
    {
        if (!AudioRecorder.HasListenedToRecording)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("Find the recording first!", "#e02a0d");
            }
            return;
        }
        
        if (taskList != null)
        {
            taskList.SetSecondPaper(false);
            taskList.ToggleTaskList();
        }
    }
}