/// <summary>
/// A simple static class to hold a global reference to the currently interrogated suspect.
/// Since it's static, it can be accessed from any other script without needing a reference.
/// </summary>
public static class SuspectManager
{
    // 'public get' allows other scripts to read who the current suspect is.
    // 'private set' means only this class can change who the current suspect is.
    public static SuspectProfile currentSuspect { get; private set; }

    /// <summary>
    /// Sets the globally tracked suspect.
    /// </summary>
    public static void SetCurrentSuspect(SuspectProfile newSuspect)
    {
        currentSuspect = newSuspect;
    }
}