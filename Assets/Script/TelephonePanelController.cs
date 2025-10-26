using Neocortex;
using Neocortex.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelephonePanelController : MonoBehaviour
{
    [Header("Suspect Data")]
    public List<SuspectProfile> suspectList = new List<SuspectProfile>();

    [Header("Required Connections")]
    public VolumeTransitionManager transitionManager;
    public Transform suspectSpawnPoint;

    [Header("UI References")]
    public GameObject suspectEntryPrefab;
    public Transform suspectListContainer;
    public Button closeButton;
    [SerializeField] private AudioChatSample chat;
    [HideInInspector]
    public TelephoneInteraction telephoneInteractor;

    private GameObject currentSuspectInstance;

    void Awake()
    {
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);
        if (transitionManager == null || suspectEntryPrefab == null || suspectListContainer == null || suspectSpawnPoint == null)
        {
            Debug.LogError("TelephonePanelController is missing one or more required references!");
            this.enabled = false;
        }
    }

    void OnEnable()
    {
        PopulateSuspectList();
        if (currentSuspectInstance == null && SuspectManager.currentSuspect != null)
        {
            //SpawnSuspect(SuspectManager.currentSuspect);
        }
    }

    public void RequestNewInterrogation(SuspectProfile profileToInterrogate)
    {
        if (SuspectManager.currentSuspect == profileToInterrogate) return;

        // Tell the interactor to update its internal state to "closed" without playing its own animation.
        telephoneInteractor.SetStateToClosed();

        // Call the new function, passing THIS panel's GameObject to be hidden instantly.
        transitionManager.PlayTransitionAndHidePanel(this.gameObject, telephoneInteractor.endInteractionSound, () =>
        {
            // This is the action at the midpoint.
            SuspectManager.SetCurrentSuspect(profileToInterrogate);
            SpawnSuspect(profileToInterrogate);
        });
    }

    private void SpawnSuspect(SuspectProfile profile)
    {
        if (currentSuspectInstance != null) Destroy(currentSuspectInstance);
        if (profile.suspectPrefab != null)
        {
            //chat.SetCurrentSuspect(profile.suspectName);
            currentSuspectInstance = Instantiate(profile.suspectPrefab, suspectSpawnPoint.position, profile.suspectPrefab.gameObject.transform.rotation);
            currentSuspectInstance.name = profile.suspectPrefab.name;
        }

        Invoke(nameof(DelayedAgentInit), 0.1f);
    }
    private void DelayedAgentInit()
    {
        if (currentSuspectInstance != null)
        {
            var agent = currentSuspectInstance.GetComponent<NeocortexSmartAgent>();
            if (agent != null)
                chat.OnAgentInitialized(agent);
        }
    }

    private void PopulateSuspectList()
    {
        foreach (Transform child in suspectListContainer) Destroy(child.gameObject);
        foreach (var suspect in suspectList)
        {
            GameObject entryGO = Instantiate(suspectEntryPrefab, suspectListContainer);
            entryGO.GetComponent<SuspectEntryUI>().Setup(suspect, this);
        }
    }

    private void ClosePanel()
    {
        if (telephoneInteractor != null) telephoneInteractor.Interact();
    }

    public void InitialSpawn()
    {
        //SpawnSuspect(SuspectManager.currentSuspect);
    }
}