using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject casePanel;

    [Header("Case UI")]
    public Image caseBackground;
    public TextMeshProUGUI caseDescription;
    public TextMeshProUGUI caseTitle;
    public Sprite[] caseImages;
    [TextArea] public string[] caseDescriptions;
    [TextArea] public string[] caseTitles;


    [SerializeField] Button startCaseButton;
    private int currentCaseIndex = 0;

    private void Start()
    {
        ShowCase(currentCaseIndex);
    }

    public void OnWorkOnNewCase()
    {
        mainMenuPanel.SetActive(false);
        casePanel.SetActive(true);
    }

    public void OnOptions()
    {
        // Puoi aprire un pannello opzioni qui (volume, ecc.)
        Debug.Log("Options clicked");
    }

    public void OnClose()
    {
        Application.Quit();
    }

    public void OnStartCase()
    {
        SceneManager.LoadScene("SolvingCase");
    }

    public void OnNextCase()
    {
        currentCaseIndex++;
        if (currentCaseIndex >= caseImages.Length)
            currentCaseIndex = 0;
        if(currentCaseIndex == (caseImages.Length - 1))
        {
            startCaseButton.gameObject.SetActive(false);
        }
        else
        {
            startCaseButton.gameObject.SetActive(true);
        }
        ShowCase(currentCaseIndex);
    }
    public void onBackToMenu() {
        casePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    private void ShowCase(int index)
    {
        caseBackground.sprite = caseImages[index];
        caseDescription.text = caseDescriptions[index];
        caseTitle.text = caseTitles[index];
    }
}
