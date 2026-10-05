using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pressAPrompt;
    [SerializeField] private GameObject chaptersMenu;
    [SerializeField] private float introDuration = 17f;
    [SerializeField] private InputActionReference confirmAction;

    [Header("Chapter 1 Transition")]
    [SerializeField] private CanvasGroup chapter1Panel;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private AudioSource chapter1Audio;
    [SerializeField] private float chapter1Wait = 31f;

    [Header("Fade Out Audio")]
    [SerializeField] private AudioSource fadeOutAudio;
    [SerializeField] private float fadeOutTime = 0.5f;

    private bool canPressA = false;
    private bool chaptersOpen = false;
    private bool transitioning = false;

    private void OnEnable()
    {
        if (confirmAction != null)
            confirmAction.action.Enable();
    }

    private void OnDisable()
    {
        if (confirmAction != null)
            confirmAction.action.Disable();
    }

    private void Start()
    {
        Cursor.visible = false;

        pressAPrompt.SetActive(false);
        chaptersMenu.SetActive(false);

        if (chapter1Panel != null)
        {
            chapter1Panel.alpha = 0f;
            chapter1Panel.blocksRaycasts = false;
            chapter1Panel.gameObject.SetActive(false);
        }

        StartCoroutine(IntroRoutine());
    }

    private void Update()
    {
        if (!canPressA || chaptersOpen || transitioning) return;

        if (confirmAction != null && confirmAction.action.WasPressedThisFrame())
        {
            OpenChaptersMenu();
        }
    }

    private IEnumerator IntroRoutine()
    {
        yield return new WaitForSeconds(introDuration);

        pressAPrompt.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        canPressA = true;
    }

    private void OpenChaptersMenu()
    {
        chaptersOpen = true;
        canPressA = false;

        Cursor.visible = true;

        pressAPrompt.SetActive(false);
        chaptersMenu.SetActive(true);
    }

    public void OnChapter1Pressed()
    {
        if (transitioning) return;
        StartCoroutine(Chapter1Routine());
    }

    private IEnumerator Chapter1Routine()
    {
        transitioning = true;

        Cursor.visible = false;

        StartCoroutine(FadeOutAudioRoutine());

        chapter1Panel.gameObject.SetActive(true);
        chapter1Panel.alpha = 0f;
        chapter1Panel.blocksRaycasts = true;

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            chapter1Panel.alpha = Mathf.Clamp01(t / fadeTime);
            yield return null;
        }
        chapter1Panel.alpha = 1f;

        if (chapter1Audio != null)
            chapter1Audio.Play();

        yield return new WaitForSeconds(chapter1Wait);

        SceneManager.LoadScene("Chapter1");
    }

    private IEnumerator FadeOutAudioRoutine()
    {
        if (fadeOutAudio == null) yield break;

        float startVolume = fadeOutAudio.volume;
        float t = 0f;

        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            fadeOutAudio.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutTime);
            yield return null;
        }

        fadeOutAudio.volume = 0f;
    }
}