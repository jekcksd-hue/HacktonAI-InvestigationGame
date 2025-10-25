using UnityEngine;
using UnityEngine.UI;
using TMPro;

// --- The SuspectProfile class now includes a reference to the 3D prefab ---
[System.Serializable]
public class SuspectProfile
{
    public string suspectName;
    public Sprite suspectImage;
    [TextArea(3, 5)]
    public string suspectDescription;

    [Tooltip("The 3D GameObject prefab for this suspect.")]
    public GameObject suspectPrefab; // <-- NEW
}

public class SuspectEntryUI : MonoBehaviour
{
    [Header("UI References")]
    public Image suspectImageComponent;
    public TextMeshProUGUI descriptionComponent;
    public Button interrogationButton;
    public TextMeshProUGUI interrogationButtonText; // <-- NEW: Reference to the button's text

    private SuspectProfile assignedProfile;
    private TelephonePanelController panelController;

    /// <summary>
    /// Populates the UI and sets the button state based on the global SuspectManager.
    /// </summary>
    public void Setup(SuspectProfile profile, TelephonePanelController controller)
    {
        assignedProfile = profile;
        panelController = controller;

        // Populate the visual elements
        suspectImageComponent.sprite = assignedProfile.suspectImage;
        descriptionComponent.text = $"<b>{assignedProfile.suspectName}</b>\n{assignedProfile.suspectDescription}";

        // --- Check the global state to set the button correctly ---
        if (SuspectManager.currentSuspect == assignedProfile)
        {
            // This is the currently interrogated suspect
            interrogationButton.interactable = false;
            interrogationButtonText.text = "Under Interrogation";
        }
        else
        {
            // This is not the currently interrogated suspect
            interrogationButton.interactable = true;
            interrogationButtonText.text = "Request An Interrogation";
        }

        // Setup the button's click listener
        interrogationButton.onClick.RemoveAllListeners();
        interrogationButton.onClick.AddListener(OnInterrogationButtonClicked);
    }

    /// <summary>
    /// When clicked, this tells the main panel controller to start the interrogation process.
    /// </summary>
    private void OnInterrogationButtonClicked()
    {
        // Call the central function on the panel controller
        panelController.RequestNewInterrogation(assignedProfile);
    }
}