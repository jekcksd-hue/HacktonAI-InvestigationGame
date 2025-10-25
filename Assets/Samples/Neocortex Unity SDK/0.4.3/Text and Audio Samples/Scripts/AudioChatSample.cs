using UnityEngine;
using Neocortex.Data;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.VisualScripting;

namespace Neocortex.Samples
{
    public class AudioChatSample : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private EmotionPopup emotionPopup;
        [Header("Neocortex Components")]
        [SerializeField] private AudioReceiver audioReceiver;
        [SerializeField] private NeocortexSmartAgent agent;
        [SerializeField] private NeocortexThinkingIndicator thinking;
        [SerializeField] private NeocortexChatPanel chatPanel;
        [SerializeField] private NeocortexAudioChatInput audioChatInput;
        [SerializeField] private CanvasGroup chatCanvasGroup;
        [Header("Text Components")]
        [SerializeField] private NeocortexTextChatInput textChatInput;

        private string currentSuspect;
        private Emotions lastEmotion = Emotions.Neutral;
        private void Awake()
        {
            audioReceiver.OnAudioRecorded.AddListener(OnAudioRecorded);
            textChatInput.OnSendButtonClicked.AddListener(OnTextSubmitted);
        }
        //private void OnEnable()
        //{
        //    audioReceiver.StartMicrophone();
        //}
        //private void OnDisable()
        //{
        //    audioReceiver.gameObject.SetActive(false);
        //}
        public void OnAgentInitialized(NeocortexSmartAgent currentAgent)
        {
            if(agent != null)
            {
                agent.OnTranscriptionReceived.RemoveListener(OnTranscriptionReceived);
                agent.OnChatResponseReceived.RemoveListener(OnChatResponseReceived);
                agent.OnAudioResponseReceived.RemoveListener(OnAudioResponseReceived);
            }
            agent = currentAgent;
            agent.CleanSessionID();
            chatPanel.ClearMessages();
            Debug.Log(agent.didAwake);
            Debug.Log(agent.didStart);
            agent.OnTranscriptionReceived.AddListener(OnTranscriptionReceived);
            agent.OnChatResponseReceived.AddListener(OnChatResponseReceived);
            agent.OnAudioResponseReceived.AddListener(OnAudioResponseReceived);
            //Invoke(nameof(ResetChatHistory),0.1f);
            Debug.Log("events loaded");
        }

        private IEnumerator ResetChatHistory()
        {
            yield return new WaitForEndOfFrame();
            chatPanel.AddMessage($"({agent.name} joiner the room)", false);
            Debug.Log("Resetting chat history...");
            yield return new WaitForEndOfFrame();
        }

        private void StartMicrophone()
        {
            audioReceiver.StartMicrophone();
        }
        
        private void OnAudioRecorded(AudioClip clip)
        {
            Debug.Log("Audio recorded, sending to agent...");
            Debug.Log(agent);
            agent.AudioToText(clip);
            Debug.Log(thinking);
            thinking.Display(true);
            Debug.Log(audioChatInput);
            audioChatInput.SetChatState(false);
        }

        private void AddMessageToHistory(string message, bool isPlayer)
        {
            Debug.Log($"Adding message to history: {(isPlayer ? "Player" : "Agent")}: {message}");
            chatPanel.AddMessage(message, isPlayer);
        }

        private void OnTranscriptionReceived(string transcription)
        {
            Debug.Log($"Transcription received: {transcription}");
            AddMessageToHistory(transcription, true);
        }

        private void OnChatResponseReceived(ChatResponse response)
        {
            
            
            string action = response.action;
            if (!string.IsNullOrEmpty(action))
            {
                Debug.Log($"[ACTION] {action}");
                switch (action)
                {
                    case "SFGHFSDRERTEGDRGHV":
                        chatPanel.AddMessage("last night i saw a asfnaj", false);
                        break;
                    // Add more actions as needed
                    default:
                        Debug.LogWarning($"Unknown action received: {action}");
                        break;
                }
            }
            else
            {
                AddMessageToHistory(response.message, false);
            }
            thinking.Display(false);
            //Emotions emotion = response.emotion;
            //if (emotion != Emotions.Neutral && emotion != lastEmotion)
            //{
            //    Debug.Log($"[EMOTION] {emotion.ToString()}");
            //    lastEmotion = emotion;
            //    if(emotionPopup == null)
            //    {
            //        emotionPopup = agent.gameObject.GetComponentInChildren<EmotionPopup>();
            //    }
            //    if(emotionPopup != null)
            //    {
            //        emotionPopup.gameObject.SetActive(true);
            //        emotionPopup.ShowEmotion(emotion.ToString());
            //    }
            //    else
            //    {
            //        Debug.LogWarning("EmotionPopup component not found on the agent.");
            //    }
            //}
        }
        private void OnAudioResponseReceived(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();

            Invoke(nameof(StartMicrophone), audioClip.length);
            
            thinking.Display(false);
            audioChatInput.SetChatState(true);
        }
        private void OnTextSubmitted(string message)
        {
            chatPanel.AddMessage(message, true);
            agent.TextToText(message);
            thinking.Display(true);
        }
        public void OnChatClose()
        {
            chatCanvasGroup.alpha = 0;
        }
    }
}
