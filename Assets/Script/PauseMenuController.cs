using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject controlsMenuUI;
    [SerializeField] private GameObject confirmReturn;
    [SerializeField] private GameObject endPanel;
    private bool isPaused = false;

    private void Update()
    {
        if (isPaused) { Debug.Log("paused"); }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RetireFromCase()
    {
        confirmReturn.SetActive(true);
    }
    public void Retire()
    {
        endPanel.SetActive(true);
        ResumeGame();
    }
    public void CancelRetire()
    {
        confirmReturn.SetActive(false);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    public void OpenControls()
    {
        controlsMenuUI.SetActive(true);
    }
    public void CloseControls() {
        controlsMenuUI.SetActive(false);
    }
}
