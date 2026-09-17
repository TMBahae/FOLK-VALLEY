using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeSpeed = 2f;
    
    private Coroutine currentNotification;
    private CanvasGroup canvasGroup;
    private Color defaultColor = Color.white;
    
    private void Awake()
    {
        canvasGroup = notificationText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = notificationText.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
        notificationText.gameObject.SetActive(true);
        defaultColor = notificationText.color;
    }
    
    private void Start()
    {
        gameObject.SetActive(true);
    }
    
    public void ShowNotification(string message)
    {
        ShowNotification(message, "#FFFFFF");
    }
    
    public void ShowNotification(string message, string colorHex)
    {
        if (currentNotification != null)
        {
            StopCoroutine(currentNotification);
        }
        
        currentNotification = StartCoroutine(DisplayNotification(message, colorHex));
    }
    
    private IEnumerator DisplayNotification(string message, string colorHex)
    {
        if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
        {
            notificationText.color = color;
        }
        else
        {
            notificationText.color = defaultColor;
        }
        
        notificationText.text = message;
        
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        yield return new WaitForSeconds(displayDuration);
        
        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        
        notificationText.color = defaultColor;
        currentNotification = null;
    }
}