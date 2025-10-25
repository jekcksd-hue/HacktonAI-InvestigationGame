using UnityEngine;

/// <summary>
/// This script manages the initial state of the investigation.
/// It now uses an index to reliably select the starting suspect from the telephone controller's list.
/// </summary>
public class InvestigationManager : MonoBehaviour
{
    [Header("Initial State Setup")]
    [Tooltip("The controller that holds the master list of all suspects.")]
    public TelephonePanelController telephoneController;

    [Tooltip("The position of the starting suspect in the Telephone Controller's list. 0 = the first suspect, 1 = the second, etc.")]
    public int startingSuspectIndex = 0; // <-- WE USE AN INDEX NOW

    void Start()
    {
        // First, check if the controller is assigned.
        if (telephoneController == null)
        {
            Debug.LogError("InvestigationManager is missing its reference to the TelephonePanelController!", this.gameObject);
            return;
        }

        // Check if the chosen index is valid for the list.
        if (startingSuspectIndex < 0 || startingSuspectIndex >= telephoneController.suspectList.Count)
        {
            Debug.LogError("The Starting Suspect Index is invalid! It must be between 0 and " + (telephoneController.suspectList.Count - 1), this.gameObject);
            return;
        }

        // Get the ACTUAL suspect profile from the master list using the index.
        //SuspectProfile startingSuspect = telephoneController.suspectList[startingSuspectIndex];

        // Set the global static reference. Now it's a direct reference, not a copy.
        //SuspectManager.SetCurrentSuspect(startingSuspect);

        //// Tell the telephone controller to spawn the initial suspect model.
        //telephoneController.InitialSpawn();
    }
}