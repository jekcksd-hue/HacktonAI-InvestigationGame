using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script manages lamp interaction. It toggles a light on/off with 'E' or
/// left-click and plays a sound effect.
/// 
/// REQUIREMENTS:
/// 1. Must be on an object with a Collider.
/// 2. 'Point Light' must be assigned.
/// 3. 'Interaction Sound' should be assigned an AudioClip.
/// 4. This script will automatically add an AudioSource component if one is not found.
/// </summary>
[RequireComponent(typeof(AudioSource))] // Automatically adds an AudioSource component to this GameObject.
public class LampInteraction : MonoBehaviour
{
    [Header("Light Settings")]
    [Tooltip("Assign the Point Light you want to toggle here.")]
    public Light pointLight;

    [Header("Audio Settings")]
    [Tooltip("The sound effect to play when the lamp is toggled.")]
    public AudioClip interactionSound;


    [SerializeField] private GameObject lightBulb;
    [SerializeField] private Material lightOn;
    [SerializeField] private Material lightOff;
    // --- Private Variables ---
    private AudioSource audioSource;
    private bool isMouseOver = false;

    void Start()
    {
        // Get the AudioSource component that is required to be on this object.
        audioSource = GetComponent<AudioSource>();
        pointLight.enabled = !pointLight.enabled;
        UpdateBulbMaterial();
        // Validate that the light has been assigned.
        if (pointLight == null)
        {
            Debug.LogError("LampInteraction Error: The 'Point Light' has not been assigned in the Inspector on " + gameObject.name);
            this.enabled = false;
        }
    }

    void Update()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }



        if (isMouseOver && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLight();
        }
    }

    private void OnMouseDown()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }



        ToggleLight();
    }

    private void ToggleLight()
    {
        if (pointLight != null)
        {
            // Toggle the light's state.
            pointLight.enabled = !pointLight.enabled;

            // Play the sound effect if it has been assigned.
            if (interactionSound != null)
            {
                // PlayOneShot is perfect for non-looping sound effects.
                audioSource.PlayOneShot(interactionSound);
            }
            UpdateBulbMaterial();
        }
    }
    private void UpdateBulbMaterial()
    {
        if (lightBulb == null || lightOn == null || lightOff == null) return;

        var renderer = lightBulb.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = pointLight.enabled ? lightOn : lightOff;
        }
    }
    // --- Mouse Hover Detection ---
    private void OnMouseEnter() { isMouseOver = true; }
    private void OnMouseExit() { isMouseOver = false; }
}