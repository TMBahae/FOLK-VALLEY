using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Outline outline;
    
    private void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
    
    public virtual void Interact()
    {
    }
    
    public virtual void OnHoverStart()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }
    
    public virtual void OnHoverEnd()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}