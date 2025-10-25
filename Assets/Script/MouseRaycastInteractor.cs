using UnityEngine;
using TMPro; // Add this line to use TextMeshPro!

/// <summary>
/// This script, applied to the camera, shoots a ray toward the mouse
/// every frame. If it hits an object on a specified layer, it displays a UI
/// with the object's name.
/// </summary>
public class MouseRaycastInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    [Tooltip("The layers that the ray should consider as interactable.")]
    public LayerMask interactableLayers;

    [Tooltip("The maximum distance the ray can travel.")]
    public float raycastDistance = 100f;

    [Header("UI References")]
    [Tooltip("The UI panel that contains the interaction text.")]
    public GameObject interactionUIPanel;

    [Tooltip("The TextMeshPro text field to update.")]
    public TextMeshProUGUI interactionText;

    // Reference to the camera (more efficient than using Camera.main every frame)
    private Camera mainCamera;

    void Start()
    {
        // Get the reference to the camera at startup
        mainCamera = GetComponent<Camera>();

        // Make sure the UI is hidden at the start of the game
        if (interactionUIPanel != null)
        {
            interactionUIPanel.SetActive(false);
        }
    }

    void Update()
    {
        // 1. Create a ray from the camera that passes through the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // 2. Declare a variable to store information about the hit object
        RaycastHit hit;

        // 3. Perform the Raycast
        // Physics.Raycast returns 'true' if it hits something, 'false' otherwise.
        // It only checks objects that belong to the layers specified in 'interactableLayers'.
        if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayers))
        {
            // --- WE HIT AN INTERACTABLE OBJECT ---

            // Check if the UI references have been assigned to avoid errors
            if (interactionUIPanel != null && interactionText != null)
            {
                // Update the UI text with the name of the object we hit
                interactionText.text = hit.collider.gameObject.name;

                // Show the UI panel
                interactionUIPanel.SetActive(true);
            }
        }
        else
        {
            // --- WE DIDN'T HIT ANYTHING (or hit an object on a non-interactable layer) ---

            // If the panel is visible, hide it
            if (interactionUIPanel != null && interactionUIPanel.activeSelf)
            {
                interactionUIPanel.SetActive(false);
            }
        }
    }
}
