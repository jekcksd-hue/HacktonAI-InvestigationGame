using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ReportInteraction : MonoBehaviour // Removed 'IInteractable' as you are using OnMouseDown
{
    [Header("UI Panel Settings")]
    public GameObject reportPanel;
    public float animationDuration = 0.4f;
    public ReportPanelController panelController;

    [Header("Audio Settings")]
    public AudioClip openSound;
    public AudioClip closeSound;

    private AudioSource audioSource;
    private bool isPanelVisible = false;
    private CanvasGroup panelCanvasGroup;
    private RectTransform panelRectTransform;
    private Coroutine activeAnimationCoroutine;
    private bool isMouseOver = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (reportPanel == null) { return; }

        // This link is still needed for the close button to work
        if (panelController != null) panelController.reportInteractor = this;

        panelCanvasGroup = reportPanel.GetComponent<CanvasGroup>();
        panelRectTransform = reportPanel.GetComponent<RectTransform>();
        if (panelCanvasGroup == null) { panelCanvasGroup = reportPanel.AddComponent<CanvasGroup>(); }
        reportPanel.SetActive(false);
        isPanelVisible = false;
    }

    // You were using OnMouseDown, so let's add that back in with the E key logic.
    void Update()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }


        if (isMouseOver && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnMouseDown()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }



        Interact();
    }

    // The public Interact() method is still useful for the panel's close button.
    public void Interact()
    {
        if (EventSystem.current.IsPointerOverGameObject() && !isPanelVisible)
        {
            return;
        }

        if (activeAnimationCoroutine != null) StopCoroutine(activeAnimationCoroutine);

        isPanelVisible = !isPanelVisible;

        if (isPanelVisible)
        {
            if (openSound != null) audioSource.PlayOneShot(openSound);
            // The line that called InitializeView() is REMOVED from here.
        }
        else
        {
            if (closeSound != null) audioSource.PlayOneShot(closeSound);
        }

        activeAnimationCoroutine = StartCoroutine(AnimatePanel(isPanelVisible));
    }

    private IEnumerator AnimatePanel(bool show)
    {
        float startAlpha = show ? 0f : 1f;
        float endAlpha = show ? 1f : 0f;
        Vector3 startScale = show ? Vector3.one * 0.7f : Vector3.one;
        Vector3 endScale = show ? Vector3.one : Vector3.one * 0.7f;
        if (show) reportPanel.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float progress = elapsedTime / animationDuration;
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            panelRectTransform.localScale = Vector3.Lerp(startScale, endScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelCanvasGroup.alpha = endAlpha;
        panelRectTransform.localScale = endScale;
        if (!show) reportPanel.SetActive(false);
    }

    private void OnMouseEnter() { isMouseOver = true; }
    private void OnMouseExit() { isMouseOver = false; }
}