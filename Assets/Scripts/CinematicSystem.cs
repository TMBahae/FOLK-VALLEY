using UnityEngine;
using TMPro;
using System.Collections;

public class CinematicSystem : MonoBehaviour
{
    public event System.Action OnCinematicFinished;
    
    [Header("Panels")]
    [SerializeField] private RectTransform topPanel;
    [SerializeField] private RectTransform bottomPanel;
    [SerializeField] private float panelSlideSpeed = 5f;
    [SerializeField] private float panelStartScale = 1f;
    [SerializeField] private float panelTargetScale = 2.7f;
    
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float startFOV = 75f;
    [SerializeField] private float targetFOV = 85f;
    [SerializeField] private float fovSpeed = 3f;
    
    [Header("Audio & Subtitles")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private string[] subtitles;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI subtitleText;
    
    [Header("References")]
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private AudioSource playerFootstepAudio;
    
    private bool isPlaying = false;
    private int currentClipIndex = 0;
    private Vector3 topStartScale;
    private Vector3 bottomStartScale;
    private Vector3 topTargetScale;
    private Vector3 bottomTargetScale;
    private CanvasGroup topCanvasGroup;
    private CanvasGroup bottomCanvasGroup;
    private CharacterController characterController;
    private bool wasControllerEnabled = true;
    
    private void Start()
    {
        topCanvasGroup = topPanel.GetComponent<CanvasGroup>();
        bottomCanvasGroup = bottomPanel.GetComponent<CanvasGroup>();
        
        topStartScale = new Vector3(panelStartScale, panelStartScale, panelStartScale);
        bottomStartScale = new Vector3(panelStartScale, panelStartScale, panelStartScale);
        topTargetScale = new Vector3(panelTargetScale, panelTargetScale, panelTargetScale);
        bottomTargetScale = new Vector3(panelTargetScale, panelTargetScale, panelTargetScale);
        
        topPanel.localScale = topStartScale;
        bottomPanel.localScale = bottomStartScale;
        
        topPanel.gameObject.SetActive(false);
        bottomPanel.gameObject.SetActive(false);
        
        if (subtitleText != null)
            subtitleText.gameObject.SetActive(false);
        
        if (mainCamera != null)
            mainCamera.fieldOfView = startFOV;
        
        if (playerController == null)
            playerController = FindObjectOfType<FirstPersonController>();
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Get CharacterController
        if (playerController != null)
            characterController = playerController.GetComponent<CharacterController>();
        
        Debug.Log("CinematicSystem initialized.");
    }
    
    public void PlayCinematic(int clipIndex)
    {
        if (isPlaying) return;
        if (clipIndex < 0 || clipIndex >= audioClips.Length)
        {
            Debug.LogWarning("Clip index out of range!");
            return;
        }
        
        if (playerController == null)
            playerController = FindObjectOfType<FirstPersonController>();
        if (characterController == null && playerController != null)
            characterController = playerController.GetComponent<CharacterController>();
        
        currentClipIndex = clipIndex;
        StartCoroutine(CinematicSequence());
    }
    
    private IEnumerator CinematicSequence()
    {
        isPlaying = true;
        
        if (playerController == null)
            playerController = FindObjectOfType<FirstPersonController>();
        if (characterController == null && playerController != null)
            characterController = playerController.GetComponent<CharacterController>();
        
        if (playerFootstepAudio != null && playerFootstepAudio.isPlaying)
            playerFootstepAudio.Stop();
        
        // FORCE LOCK PLAYER - Disable CharacterController
        if (characterController != null)
        {
            wasControllerEnabled = characterController.enabled;
            characterController.enabled = false;
            Debug.Log("Cinematic: CharacterController DISABLED");
        }
        
        // Also lock via script
        if (playerController != null)
        {
            playerController.CanMove = false;
            playerController.CanLook = true;
        }
        
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        SetPanelAlpha(1f);
        
        yield return StartCoroutine(ScalePanels(true));
        yield return StartCoroutine(ChangeFOV(targetFOV));
        
        if (audioSource != null && audioClips[currentClipIndex] != null)
        {
            audioSource.clip = audioClips[currentClipIndex];
            if (subtitleText != null && subtitles.Length > currentClipIndex)
            {
                subtitleText.gameObject.SetActive(true);
                subtitleText.text = subtitles[currentClipIndex];
            }
            audioSource.Play();
            while (audioSource.isPlaying)
                yield return null;
            if (subtitleText != null)
                subtitleText.gameObject.SetActive(false);
        }
        
        yield return StartCoroutine(ChangeFOV(startFOV));
        yield return StartCoroutine(ScalePanels(false));
        
        topPanel.gameObject.SetActive(false);
        bottomPanel.gameObject.SetActive(false);
        
        // UNLOCK PLAYER - Re-enable CharacterController
        if (characterController != null)
        {
            characterController.enabled = wasControllerEnabled;
            Debug.Log("Cinematic: CharacterController RE-ENABLED");
        }
        
        if (playerController != null)
        {
            playerController.CanMove = true;
            playerController.CanLook = true;
        }
        
        isPlaying = false;
        
        if (OnCinematicFinished != null)
            OnCinematicFinished.Invoke();
    }
    
    private void SetPanelAlpha(float alpha)
    {
        if (topCanvasGroup != null) topCanvasGroup.alpha = alpha;
        if (bottomCanvasGroup != null) bottomCanvasGroup.alpha = alpha;
    }
    
    private IEnumerator ScalePanels(bool scaleIn)
    {
        Vector3 topTarget = scaleIn ? topTargetScale : topStartScale;
        Vector3 bottomTarget = scaleIn ? bottomTargetScale : bottomStartScale;
        Vector3 topCurrent = topPanel.localScale;
        Vector3 bottomCurrent = bottomPanel.localScale;
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * panelSlideSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, timer);
            topPanel.localScale = Vector3.Lerp(topCurrent, topTarget, eased);
            bottomPanel.localScale = Vector3.Lerp(bottomCurrent, bottomTarget, eased);
            yield return null;
        }
        topPanel.localScale = topTarget;
        bottomPanel.localScale = bottomTarget;
    }
    
    private IEnumerator ChangeFOV(float targetFOV)
    {
        float startFOV = mainCamera.fieldOfView;
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fovSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, timer);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, eased);
            yield return null;
        }
        mainCamera.fieldOfView = targetFOV;
    }
}