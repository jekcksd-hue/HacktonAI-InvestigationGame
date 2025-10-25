using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PageContent
{
    [TextArea(5, 10)]
    public string leftPageText;

    [TextArea(5, 10)]
    public string rightPageText;
}

public class ReportPanelController : MonoBehaviour
{
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    [SerializeField] GameObject page3;
    [SerializeField] GameObject page4;


    [Header("Button References")]
    public Button nextPageButton;
    public Button previousPageButton;
    public Button closeButton;

    [HideInInspector]
    public ReportInteraction reportInteractor;

    private int currentPageIndex = 1;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactionSound;


    /// <summary>
    /// OnEnable is a special Unity function that runs EVERY TIME this GameObject is set to active.
    /// This is the perfect place to reset the panel to its initial state.
    /// This solves both the "no page on first open" and "doesn't reset" problems.
    /// </summary>
    void OnEnable()
    {
        // Reset to the first page every time the panel is enabled.
        currentPageIndex = 1;
        DisplayPage(currentPageIndex);
    }

    private void DisplayPage(int pageIndex)
    {
        switch (currentPageIndex)
        {
            case 1: DisableAllPages(); page1.SetActive(true); nextPageButton.gameObject.SetActive(true);previousPageButton.gameObject.SetActive(false); audioSource.PlayOneShot(interactionSound); break;
            case 2: DisableAllPages(); page2.SetActive(true); nextPageButton.gameObject.SetActive(true); previousPageButton.gameObject.SetActive(true); audioSource.PlayOneShot(interactionSound); break;
            case 3: DisableAllPages(); page3.SetActive(true); nextPageButton.gameObject.SetActive(true); previousPageButton.gameObject.SetActive(true); audioSource.PlayOneShot(interactionSound); break;
            case 4: DisableAllPages(); page4.SetActive(true); nextPageButton.gameObject.SetActive(false); previousPageButton.gameObject.SetActive(true); audioSource.PlayOneShot(interactionSound); break;
            default:
                break;
        }
    }

    private void DisableAllPages()
    {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
    }


    public void GoToNextPage()
    {
        currentPageIndex++;
        DisplayPage(currentPageIndex);
    }

    public void GoToPreviousPage()
    {
        currentPageIndex--;
        DisplayPage(currentPageIndex);
    }

    public void ClosePanel()
    {
        if (reportInteractor != null)
        {
            reportInteractor.Interact();
        }
    }
}