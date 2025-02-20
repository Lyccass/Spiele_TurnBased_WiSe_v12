using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance { get; private set; }

    [SerializeField] private GameObject tooltipUI;         // Tooltip UI GameObject
    [SerializeField] private TextMeshProUGUI tooltipText;  // TextMeshPro component for tooltip text
    [SerializeField] private CanvasGroup canvasGroup;      // CanvasGroup for fading
    [SerializeField] private float hoverDelay = .5f;      // Delay before showing tooltip
    [SerializeField] private float fadeDuration = 0.5f;    // Duration for fade-in

    private RectTransform tooltipRectTransform;
    private Coroutine showTooltipCoroutine;
    private bool isHovering;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Tooltip instances found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        tooltipRectTransform = tooltipUI.GetComponent<RectTransform>();
        HideTooltip();
    }

    private void Update()
    {
        if (tooltipUI.activeSelf)
        {
            Vector2 mousePosition = InputManager.Instance.GetMouseScreenPosition();
            tooltipRectTransform.position = mousePosition;
        }
    }

    public void StartHover(string message)
    {
        isHovering = true;

        // Cancel any ongoing tooltip show coroutine
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
        }

        // Start the tooltip after a delay
        showTooltipCoroutine = StartCoroutine(ShowTooltipWithDelay(message));
    }

    public void StopHover()
    {
        isHovering = false;

        // Cancel any ongoing tooltip show coroutine
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
        }

        // Hide the tooltip immediately
        HideTooltip();
    }

    private IEnumerator ShowTooltipWithDelay(string message)
    {
        yield return new WaitForSeconds(hoverDelay);

        if (isHovering) // Ensure the cursor is still hovering
        {
            ShowTooltip(message);
        }
    }

    private void ShowTooltip(string message)
    {
        tooltipText.text = message;
        tooltipUI.SetActive(true);

        if (canvasGroup != null)
        {
            StartCoroutine(FadeInTooltip());
        }
    }

    private IEnumerator FadeInTooltip()
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
    }

    public void HideTooltip()
    {
        tooltipUI.SetActive(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }
}
