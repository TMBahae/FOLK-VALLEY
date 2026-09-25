using UnityEngine;

public class Cleanable : Interactable
{
    [SerializeField] CleanableManager manager;

    public override void Interact()
    {
        manager.OnClean();
        Destroy(gameObject);
    }
}
