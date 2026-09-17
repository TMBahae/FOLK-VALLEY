using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TaskList : MonoBehaviour
{
    [Header("First Paper")]
    [SerializeField] private CanvasGroup firstBG;
    [SerializeField] private RectTransform firstPaper;

    [Header("Second Paper")]
    [SerializeField] private CanvasGroup secondBG;
    [SerializeField] private RectTransform secondPaper;

    [Header("Animation Settings")]
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float paperSlideSpeed = 5f;
    [SerializeField] private float paperTargetY = 0f;
    [SerializeField] private float paperStartY = -1000f;
    [SerializeField] private float minCloseDelay = 3f;

    private bool isOpen = false;
    private bool isAnimating = false;
    private bool canClose = false;
    private bool isSecondPaper = false;
    private FirstPersonController playerController;
    private AudioSource footstepAudio;

    private CanvasGroup currentBG;
    private RectTransform currentPaper;

    public bool IsOpen => isOpen;

    private void Start()
    {
        firstBG.alpha = 0f;
        secondBG.alpha = 0f;
        firstPaper.anchoredPosition = new Vector2(0f, paperStartY);
        secondPaper.anchoredPosition = new Vector2(0f, paperStartY);

        firstBG.gameObject.SetActive(false);
        secondBG.gameObject.SetActive(false);

        playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
            footstepAudio = playerController.GetComponent<AudioSource>();

        currentBG = firstBG;
        currentPaper = firstPaper;
    }

    private void Update()
    {
        if (isOpen && !isAnimating && canClose && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            CloseTaskList();
    }

    public void ToggleTaskList()
    {
        if (isAnimating) return;
        if (isOpen)
            CloseTaskList();
        else
            OpenTaskList();
    }

    public void OpenTaskList()
    {
        if (isOpen || isAnimating) return;
        isOpen = true;
        canClose = false;

        if (playerController != null)
        {
            playerController.CanMove = false;
            playerController.CanLook = false;
        }

        if (footstepAudio != null && footstepAudio.isPlaying)
            footstepAudio.Stop();

        currentBG.gameObject.SetActive(true);
        currentBG.alpha = 0f;
        currentPaper.anchoredPosition = new Vector2(0f, paperStartY);

        StartCoroutine(OpenAnimation());
        StartCoroutine(EnableCloseAfterDelay());
    }

    public void CloseTaskList()
    {
        if (!isOpen || isAnimating || !canClose) return;
        isOpen = false;
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator EnableCloseAfterDelay()
    {
        yield return new WaitForSeconds(minCloseDelay);
        canClose = true;
    }

    public void SetSecondPaper(bool value)
    {
        isSecondPaper = value;
        currentBG = isSecondPaper ? secondBG : firstBG;
        currentPaper = isSecondPaper ? secondPaper : firstPaper;
    }

    private IEnumerator OpenAnimation()
    {
        isAnimating = true;
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            currentBG.alpha = Mathf.Lerp(0f, 1f, timer);
            yield return null;
        }
        currentBG.alpha = 1f;

        timer = 0f;
        Vector2 startPos = new Vector2(0f, paperStartY);
        Vector2 endPos = new Vector2(0f, paperTargetY);
        while (timer < 1f)
        {
            timer += Time.deltaTime * paperSlideSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, timer);
            currentPaper.anchoredPosition = Vector2.Lerp(startPos, endPos, eased);
            yield return null;
        }
        currentPaper.anchoredPosition = endPos;
        isAnimating = false;
    }

    private IEnumerator CloseAnimation()
    {
        isAnimating = true;
        float timer = 0f;
        Vector2 startPos = new Vector2(0f, paperTargetY);
        Vector2 endPos = new Vector2(0f, paperStartY);
        while (timer < 1f)
        {
            timer += Time.deltaTime * paperSlideSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, timer);
            currentPaper.anchoredPosition = Vector2.Lerp(startPos, endPos, eased);
            yield return null;
        }
        currentPaper.anchoredPosition = endPos;

        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            currentBG.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return null;
        }
        currentBG.alpha = 0f;
        currentBG.gameObject.SetActive(false);

        if (playerController != null)
        {
            playerController.CanMove = true;
            playerController.CanLook = true;
        }

        isAnimating = false;
        canClose = false;
    }
}