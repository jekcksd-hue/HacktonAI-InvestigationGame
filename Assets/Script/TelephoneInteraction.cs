using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script handles interaction with the Telephone GameObject.
/// When activated by 'E' key or left-click, it displays a UI panel
/// with a smooth fade-in and scale-up animation, and plays a sound.
/// Interacting again will hide the panel and play a different sound.
///
/// REQUIREMENTS:
/// 1. This script must be on the interactive Telephone object.
/// 2. The Telephone object MUST have a Collider component (e.g., BoxCollider).
/// 3. The 'Telephone UI Panel' field must be assigned in the Inspector.
/// 4. The assigned UI Panel MUST have a 'Canvas Group' component for fading.
/// </summary>
[RequireComponent(typeof(AudioSource))] // Automatically adds an AudioSource if one isn't present.
public class TelephoneInteraction : MonoBehaviour
{
    [Header("UI Panel Settings")]
    [Tooltip("The UI Panel to show/hide. It must have a Canvas Group component.")]
    public GameObject telephoneUIPanel;

    [Tooltip("How long the fade and scale animation should take in seconds.")]
    public float animationDuration = 0.4f;

    [Tooltip("The controller script that is on the UI Panel itself.")]
    public TelephonePanelController panelController;



    [Header("Audio Settings")]
    [Tooltip("Sound to play when the interaction starts (e.g., picking up the phone).")]
    public AudioClip startInteractionSound;

    [Tooltip("Sound to play when the interaction ends (e.g., hanging up).")]
    public AudioClip endInteractionSound;

    // --- Private Variables ---
    private AudioSource audioSource;
    private bool isMouseOver = false;
    private bool isPanelVisible = false;
    private CanvasGroup panelCanvasGroup;
    private RectTransform panelRectTransform;
    private Coroutine activeAnimationCoroutine;

    void Start()
    {
        // Get the required AudioSource component.
        audioSource = GetComponent<AudioSource>();

        // --- Initial Setup and Validation ---
        if (telephoneUIPanel == null)
        {
            Debug.LogError("TelephoneInteraction Error: The 'Telephone UI Panel' has not been assigned on " + gameObject.name);
            this.enabled = false;
            return;
        }

        if (panelController != null) panelController.telephoneInteractor = this;


        // Get the required components from the panel.
        panelCanvasGroup = telephoneUIPanel.GetComponent<CanvasGroup>();
        panelRectTransform = telephoneUIPanel.GetComponent<RectTransform>();

        // Add a Canvas Group automatically if it's missing.
        if (panelCanvasGroup == null)
        {
            Debug.LogWarning("TelephoneInteraction Warning: No Canvas Group found on the panel. Adding one automatically.");
            panelCanvasGroup = telephoneUIPanel.AddComponent<CanvasGroup>();
        }

        // Ensure the panel is hidden at the start.
        telephoneUIPanel.SetActive(false);
        isPanelVisible = false;
    }

    void Update()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }


        // Handle 'E' key press while hovering.
        if (isMouseOver && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // Handle left mouse click.
    private void OnMouseDown()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }


        Interact();
    }


    public void Interact()
    {
        if (EventSystem.current.IsPointerOverGameObject() && !isPanelVisible) { return; }
        if (activeAnimationCoroutine != null) StopCoroutine(activeAnimationCoroutine);
        isPanelVisible = !isPanelVisible;
        if (isPanelVisible) { if (startInteractionSound != null) audioSource.PlayOneShot(startInteractionSound); }
        else { if (endInteractionSound != null) audioSource.PlayOneShot(endInteractionSound); }
        activeAnimationCoroutine = StartCoroutine(AnimatePanel(isPanelVisible));
    }





    /// <summary>
    /// Coroutine that animates the panel's alpha (fade) and scale over time.
    /// </summary>
    /// <param name="show">If true, animates the panel into view. If false, animates it out of view.</param>
    private IEnumerator AnimatePanel(bool show)
    {
        float startAlpha = show ? 0f : 1f;
        float endAlpha = show ? 1f : 0f;
        Vector3 startScale = show ? Vector3.one * 0.7f : Vector3.one;
        Vector3 endScale = show ? Vector3.one : Vector3.one * 0.7f;

        if (show) telephoneUIPanel.SetActive(true);

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

        if (!show) telephoneUIPanel.SetActive(false);
    }

    /// <summary>
    /// Called by the panel controller to synchronize the state when a transition is happening.
    /// This prevents the panel from trying to play its own animation.
    /// </summary>
    public void SetStateToClosed()
    {
        isPanelVisible = false;
        if (activeAnimationCoroutine != null)
        {
            StopCoroutine(activeAnimationCoroutine);
        }
    }


    // --- Mouse Hover Detection ---
    private void OnMouseEnter() { isMouseOver = true; }
    private void OnMouseExit() { isMouseOver = false; }
}