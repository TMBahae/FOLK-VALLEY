using UnityEngine;
using System.Collections;

public class Paper : Interactable
{
    [SerializeField] private TaskList taskList;
    [SerializeField] private CinematicSystem cinematicSystem;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private int cinematicIndex = 5;

    private bool isWaitingForClose = false;
    private bool cinematicPlaying = false;

    public override void Interact()
    {
        if (cinematicPlaying) return;

        if (!AudioRecorder.HasListenedToRecording)
            return;

        if (!KeyManager.KeysFound)
            return;

        if (taskList != null)
        {
            taskList.SetSecondPaper(true);
            taskList.ToggleTaskList();
            isWaitingForClose = true;
        }

        if (objectToEnable != null)
            objectToEnable.SetActive(true);
    }

    private void Update()
    {
        if (!isWaitingForClose) return;
        if (cinematicPlaying) return;
        if (taskList == null) return;

        if (!taskList.IsOpen)
        {
            isWaitingForClose = false;

            if (cinematicSystem != null)
            {
                cinematicPlaying = true;
                StartCoroutine(PlayCinematicAndWait());
            }
        }
    }

    private IEnumerator PlayCinematicAndWait()
    {
        bool finished = false;
        cinematicSystem.OnCinematicFinished += () => { finished = true; };

        cinematicSystem.PlayCinematic(cinematicIndex);

        while (!finished)
            yield return null;

        cinematicPlaying = false;
    }
}