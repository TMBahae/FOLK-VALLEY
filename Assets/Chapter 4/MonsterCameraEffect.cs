using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Bildschirmflackern mit gleichzeitig laufender Schwarzblende.
// Auf ein aktives GameObject ziehen, z. B. die Spieler-Kamera.
// Benoetigt Unity UI (uGUI); funktioniert auch mit URP.
[DisallowMultipleComponent]
public sealed class MonsterCameraEffect : MonoBehaviour
{
    [Header("Flackern")]
    [Min(0f)] public float flickerDuration = 2f;
    [Range(0f, 1f)] public float flickerStrength = 0.95f;
    [Min(0.01f)] public float minInterval = 0.04f;
    [Min(0.01f)] public float maxInterval = 0.12f;

    [Header("Schwarzblende")]
    [Tooltip("Zeit ab Effektstart bis das Bild vollstaendig schwarz ist.")]
    [Min(0f)] public float fadeDuration = 3f;

    [Header("Test")]
    [Tooltip("Startet den Effekt sofort beim Start der Szene.")]
    public bool playOnStart;

    private Canvas overlayCanvas;
    private CanvasGroup overlayGroup;
    private bool isPlaying;

    private void Awake()
    {
        CreateOverlay();
    }

    private void Start()
    {
        if (playOnStart)
            PlayEffect();
    }

    // In einem Monster-/Trigger-Skript oder einem UnityEvent aufrufen.
    [ContextMenu("Play Effect (im Play Mode)")]
    public void PlayEffect()
    {
        if (!Application.isPlaying || !isActiveAndEnabled || isPlaying)
            return;

        CreateOverlay();
        isPlaying = true;
        overlayGroup.alpha = 0f;
        overlayCanvas.enabled = true;
        StartCoroutine(EffectSequence());
    }

    // Entfernt auch ein bereits vollstaendig schwarzes Bild.
    [ContextMenu("Reset Effect (im Play Mode)")]
    public void ResetEffect()
    {
        StopAllCoroutines();
        isPlaying = false;

        if (overlayGroup != null)
            overlayGroup.alpha = 0f;

        if (overlayCanvas != null)
            overlayCanvas.enabled = false;
    }

    private IEnumerator EffectSequence()
    {
        float flickerTime = Mathf.Max(0f, flickerDuration);
        float fadeTime = Mathf.Max(0f, fadeDuration);
        float totalDuration = Mathf.Max(flickerTime, fadeTime);
        float strength = Mathf.Clamp01(flickerStrength);
        float shortest = Mathf.Max(0.01f, minInterval);
        float longest = Mathf.Max(shortest, maxInterval);
        float elapsed = 0f;
        float untilNextChange = 0f;
        float flickerAlpha = 0f;
        bool darkFrame = false;

        while (elapsed < totalDuration)
        {
            if (elapsed < flickerTime)
            {
                if (untilNextChange <= 0f)
                {
                    darkFrame = !darkFrame;
                    flickerAlpha = darkFrame
                        ? Random.Range(strength * 0.65f, strength)
                        : Random.Range(0f, strength * 0.08f);

                    untilNextChange = Random.Range(shortest, longest);
                }
            }
            else
            {
                flickerAlpha = 0f;
            }

            // Die Schwarzblende laeuft ab Effektstart parallel zum Flackern.
            float fadeAlpha = fadeTime <= 0f
                ? 1f
                : Mathf.SmoothStep(0f, 1f, elapsed / fadeTime);

            // Flackern darf zusaetzlich abdunkeln, aber den Fade nie aufheben.
            overlayGroup.alpha = Mathf.Lerp(fadeAlpha, 1f, flickerAlpha);

            yield return null;
            float step = Time.unscaledDeltaTime;
            elapsed += step;
            untilNextChange -= step;
        }

        overlayGroup.alpha = 1f;
        isPlaying = false;
        // Bleibt schwarz, bis ResetEffect() aufgerufen wird.
    }

    private void CreateOverlay()
    {
        if (overlayCanvas != null)
            return;

        // Eigenes Root-Canvas: keine Aenderung am vorhandenen UI notwendig.
        GameObject root = new GameObject(
            "Monster Camera Overlay", typeof(Canvas), typeof(CanvasGroup));

        // Gleiche Szene wie dieses Skript, auch bei additiv geladenen Szenen.
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(
            root, gameObject.scene);

        overlayCanvas = root.GetComponent<Canvas>();
        overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        overlayCanvas.sortingOrder = 32760;

        Camera targetCamera = GetComponent<Camera>();
        if (targetCamera == null)
            targetCamera = Camera.main;
        if (targetCamera != null)
            overlayCanvas.targetDisplay = targetCamera.targetDisplay;

        overlayGroup = root.GetComponent<CanvasGroup>();
        overlayGroup.alpha = 0f;
        overlayGroup.interactable = false;
        overlayGroup.blocksRaycasts = false;

        GameObject panel = new GameObject(
            "Black Screen", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(root.transform, false);

        Image image = panel.GetComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = false;

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        overlayCanvas.enabled = false;
    }

    private void OnDisable()
    {
        ResetEffect();
    }

    private void OnDestroy()
    {
        if (overlayCanvas != null)
            Destroy(overlayCanvas.gameObject);
    }
}
