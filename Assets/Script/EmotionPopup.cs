using UnityEngine;
using TMPro;
using System.Collections;

public class EmotionPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI emotionText;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuration = 0.15f;
    [SerializeField] private float moveOffset = 30f; // distanza movimento verticale in px

    private Coroutine currentRoutine;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        canvasGroup.alpha = 0;
    }

    public void ShowEmotion(string emotion)
    {
        emotionText.text = emotion;
        emotionText.color = GetEmotionColor(emotion);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        // POP scale
        transform.localScale = originalScale * popScale;
        canvasGroup.alpha = 0;

        // POP bounce
        float t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, t / popDuration);
            yield return null;
        }
        transform.localScale = originalScale;

        // MOVE + FADE IN
        t = 0f;
        Vector3 startPos = originalPosition - new Vector3(0, moveOffset, 0);
        Vector3 endPos = originalPosition;
        transform.localPosition = startPos;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, p);
            canvasGroup.alpha = Mathf.Lerp(0, 1, p);
            yield return null;
        }
        transform.localPosition = endPos;
        canvasGroup.alpha = 1;

        // WAIT
        yield return new WaitForSeconds(displayTime);

        // MOVE + FADE OUT
        t = 0f;
        Vector3 outPos = originalPosition + new Vector3(0, moveOffset, 0);

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            transform.localPosition = Vector3.Lerp(endPos, outPos, p);
            canvasGroup.alpha = Mathf.Lerp(1, 0, p);
            yield return null;
        }
        transform.localPosition = originalPosition;
        canvasGroup.alpha = 0;
    }

    private Color GetEmotionColor(string emotion)
    {
        switch (emotion)
        {
            case "Happy": return new Color(0.2f, 0.8f, 0.2f);
            case "Pleased": return new Color(0.4f, 0.9f, 0.4f);
            case "Disappointed": return new Color(0.5f, 0.5f, 0.5f);
            case "Upset": return new Color(0.8f, 0.3f, 0.3f);

            case "Amazed": return new Color(1f, 0.85f, 0.2f);
            case "Curious": return new Color(0.3f, 0.7f, 1f);
            case "Confused": return new Color(0.6f, 0.4f, 1f);
            case "Alarmed": return new Color(1f, 0.4f, 0.2f);

            case "Fascinated": return new Color(0.2f, 0.8f, 1f);
            case "Impressed": return new Color(0.6f, 0.9f, 0.6f);
            case "Annoyed": return new Color(1f, 0.55f, 0f);
            case "Angry": return new Color(0.9f, 0.2f, 0.2f);

            case "Confident": return new Color(0.2f, 0.6f, 1f);
            case "Reassured": return new Color(0.3f, 0.8f, 0.6f);
            case "Concerned": return new Color(0.9f, 0.7f, 0.3f);
            case "Scared": return new Color(0.6f, 0.2f, 0.8f);

            case "Neutral":
            default: return Color.white;
        }
    }
}
