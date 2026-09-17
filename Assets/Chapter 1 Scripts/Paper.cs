using UnityEngine;
using System.Collections;

public class Paper : Interactable
{
    [SerializeField] private TaskList taskList;
    [SerializeField] private CinematicSystem cinematicSystem;
    [SerializeField] private NotificationSystem notificationSystem;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private int cinematicIndex = 5;

    private bool isWaitingForClose = false;
    private bool cinematicPlaying = false;

    public override void Interact()
    {
        if (!AudioRecorder.HasListenedToRecording)
        {
            if (notificationSystem != null)
                notificationSystem.ShowNotification("Find the recording first!", "#e02a0d");
            return;
        }

        if (!KeyManager.KeysFound)
        {
            if (notificationSystem != null)
                notificationSystem.ShowNotification("Keys required to unlock!", "#e02a0d");
            return;
        }

        if (taskList != null)
        {
            taskList.SetSecondPaper(true);
            taskList.ToggleTaskList();
            isWaitingForClose = true;
            Debug.Log("Paper: Opened.");
        }
    }

    private void Update()
    {
        if (!isWaitingForClose) return;
        if (taskList == null) return;
        if (cinematicPlaying) return;

        if (!taskList.IsOpen && isWaitingForClose)
        {
            Debug.Log("Paper: Closed. Starting cinematic...");
            isWaitingForClose = false;
            cinematicPlaying = true;

            if (cinematicSystem != null)
            {
                StartCoroutine(PlayCinematicAndWait());
            }
            else
            {
                Debug.LogError("Paper: CinematicSystem is null!");
                cinematicPlaying = false;
            }
        }
    }

    private IEnumerator PlayCinematicAndWait()
    {
        Debug.Log("Paper: Playing cinematic...");
        
        bool finished = false;
        cinematicSystem.OnCinematicFinished += () => { finished = true; };
        
        cinematicSystem.PlayCinematic(cinematicIndex);
        
        while (!finished)
            yield return null;
        
        Debug.Log("Paper: Cinematic finished. Enabling object.");
        
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
            Debug.Log("Paper: Object enabled: " + objectToEnable.name);
        }

        if (notificationSystem != null)
            notificationSystem.ShowNotification("Cube unlocked!", "#b1fc03");

        cinematicPlaying = false;
    }
}