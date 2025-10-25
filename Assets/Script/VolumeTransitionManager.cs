using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

[RequireComponent(typeof(AudioSource))]
public class VolumeTransitionManager : MonoBehaviour
{
    [Header("Required Connections")]
    public Volume sceneVolume;
    [Tooltip("The full-screen black UI panel with a Canvas Group on it.")]
    public CanvasGroup blackoutCanvasGroup; // <-- THE NEW REFERENCE

    [Header("Transition Settings")]
    public float transitionDuration = 0.5f;

    private Vignette vignette;
    private float initialIntensity;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Add a check for the new reference
        if (sceneVolume == null || blackoutCanvasGroup == null)
        {
            Debug.LogError("VolumeTransitionManager is missing a reference to the Scene Volume or Blackout Canvas Group!", this.gameObject);
            this.enabled = false;
            return;
        }
        if (!sceneVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("The assigned Volume Profile does not have a Vignette effect!", this.gameObject);
            this.enabled = false;
            return;
        }
        initialIntensity = vignette.intensity.value;
    }

    public void PlayTransitionAndHidePanel(GameObject panelToHide, AudioClip transitionSound, Action actionAtMidpoint)
    {
        StartCoroutine(AnimateVignette(panelToHide, transitionSound, actionAtMidpoint));
    }

    private IEnumerator AnimateVignette(GameObject panelToHide, AudioClip sound, Action actionAtMidpoint)
    {
        if (sound != null) audioSource.PlayOneShot(sound);
        if (panelToHide != null) panelToHide.SetActive(false);

        // --- FADE TO BLACK ---
        // Animate both the vignette intensity UP and the blackout panel alpha UP.
        yield return AnimateEffects(initialIntensity, 1.0f, 0.0f, 1.0f);

        // --- MIDPOINT ---
        actionAtMidpoint?.Invoke();
        yield return new WaitForSeconds(0.1f);

        // --- FADE TO SCENE ---
        // Animate both the vignette intensity DOWN and the blackout panel alpha DOWN.
        yield return AnimateEffects(1.0f, initialIntensity, 1.0f, 0.0f);
    }

    /// <summary>
    /// A new, combined animation coroutine for both effects.
    /// </summary>
    private IEnumerator AnimateEffects(float startIntensity, float endIntensity, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float progress = elapsedTime / transitionDuration;
            // Animate both properties simultaneously in the same loop.
            vignette.intensity.value = Mathf.Lerp(startIntensity, endIntensity, progress);
            blackoutCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the final values are set perfectly.
        vignette.intensity.value = endIntensity;
        blackoutCanvasGroup.alpha = endAlpha;
    }
}