using Neocortex;
using UnityEngine;

public class NeocortexSmartAgentFactory : NeocortexSmartAgent
{
    public void SetSessionID(string sessionId)
    {
        PlayerPrefs.SetString("neocortex-session-id", sessionId);
    }
    public string GetSessionID(string sessionId)
    {
        return PlayerPrefs.GetString("neocortex-session-id", sessionId);
    }

    public void CleanSessionID(string sessionId)
    {
        PlayerPrefs.SetString("neocortex-session-id", sessionId);
    }
}
