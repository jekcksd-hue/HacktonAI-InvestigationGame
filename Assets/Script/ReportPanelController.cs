using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    [Header("Page Content")]
    public List<PageContent> pages = new List<PageContent>();

    [Header("UI Element References")]
    public TextMeshProUGUI leftTextArea;
    public TextMeshProUGUI rightTextArea;

    [Header("Button References")]
    public Button nextPageButton;
    public Button previousPageButton;
    public Button closeButton;

    [HideInInspector]
    public ReportInteraction reportInteractor;

    private int currentPageIndex = 0;

    void Awake()
    {
        if (nextPageButton != null) nextPageButton.onClick.AddListener(GoToNextPage);
        if (previousPageButton != null) previousPageButton.onClick.AddListener(GoToPreviousPage);
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);
    }

    /// <summary>
    /// OnEnable is a special Unity function that runs EVERY TIME this GameObject is set to active.
    /// This is the perfect place to reset the panel to its initial state.
    /// This solves both the "no page on first open" and "doesn't reset" problems.
    /// </summary>
    void OnEnable()
    {
        // Reset to the first page every time the panel is enabled.
        currentPageIndex = 0;
        DisplayPage(currentPageIndex);
    }

    private void DisplayPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Count)
        {
            // If there are no pages defined, clear the text to avoid showing placeholder content.
            if (pages.Count == 0)
            {
                leftTextArea.text = "";
                rightTextArea.text = "";
            }
            return;
        }

        leftTextArea.text = pages[pageIndex].leftPageText;
        rightTextArea.text = pages[pageIndex].rightPageText;

        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        // --- THIS IS THE FIX FOR BUTTON VISIBILITY ---
        // Instead of setting 'interactable', we enable/disable the entire button GameObject.

        // The 'previous' button should only be VISIBLE if we are not on the first page.
        if (previousPageButton != null)
        {
            previousPageButton.gameObject.SetActive(currentPageIndex > 0);
        }

        // The 'next' button should only be VISIBLE if we are not on the last page.
        if (nextPageButton != null)
        {
            nextPageButton.gameObject.SetActive(currentPageIndex < pages.Count - 1);
        }
    }

    private void GoToNextPage()
    {
        if (currentPageIndex < pages.Count - 1)
        {
            currentPageIndex++;
            DisplayPage(currentPageIndex);
        }
    }

    private void GoToPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayPage(currentPageIndex);
        }
    }

    private void ClosePanel()
    {
        if (reportInteractor != null)
        {
            reportInteractor.Interact();
        }
    }
}