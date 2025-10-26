using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup titleGroup;
    public CanvasGroup descriptionGroup;

    public CanvasGroup descriptionText;

    public CanvasGroup buttonExit;

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float delayBetween = 1f;
    public string thankYouSceneName = "ThankYouScene";
    private void OnEnable()
    {
        ShowEndSequence();
    }

    public void ShowEndSequence()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // Step 1: Title
        yield return FadeIn(titleGroup);
        yield return new WaitForSeconds(delayBetween);

        // Step 2: DescriptionText
        yield return FadeIn(descriptionText);
        yield return new WaitForSeconds(delayBetween);

        // Step 2: DescriptionImg
        yield return FadeIn(descriptionGroup);
        yield return new WaitForSeconds(delayBetween);




        yield return FadeIn(buttonExit);
        yield return new WaitForSeconds(delayBetween);
        //SceneManager.LoadScene(thankYouSceneName);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator FadeIn(CanvasGroup group)
    {
        group.gameObject.SetActive(true);
        group.alpha = 0;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        group.alpha = 1;
    }
}
