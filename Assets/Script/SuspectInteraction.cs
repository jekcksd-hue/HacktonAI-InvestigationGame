using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))] 
public class SuspectInteraction : MonoBehaviour
{

    [Header("Audio Settings")]
    [Tooltip("The sound effect to play when the lamp is toggled.")]
    public AudioClip interactionSound;


    // --- Private Variables ---
    private AudioSource audioSource;
    private bool isMouseOver = false;
    [SerializeField] private GameObject chatPanel;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (chatPanel == null)
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.CompareTag("ChatCanvas"))
                {
                    chatPanel = go;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }



        if (isMouseOver && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChat();
        }
    }

    private void OnMouseDown()
    {
        // ADD THIS CHECK: If the mouse is over a UI element, do nothing.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Stop the function here
        }
        ToggleChat();
    }

    private void ToggleChat()
    {
        if (chatPanel != null)
        {
            chatPanel.GetComponent<CanvasGroup>().alpha = 1f;
            chatPanel.transform.localScale = Vector3.one;
            if (interactionSound != null)
            {
                audioSource.PlayOneShot(interactionSound);
            }
        }
    }

    private void OnMouseEnter() { isMouseOver = true; }
    private void OnMouseExit() { isMouseOver = false; }
}