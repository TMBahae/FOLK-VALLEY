using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    
    public void Show(string message)
    {
        promptText.text = message;
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}