using UnityEngine;

public class BroomScript : Interactable
{
    private static bool hasPickedUpBroom = false;
    public static bool HasPickedUpBroom => hasPickedUpBroom;

    [SerializeField] private GameObject broom;

    public override void Interact()
    {
        hasPickedUpBroom = true;
        broom.SetActive(true);
        Destroy(gameObject);
    }
}