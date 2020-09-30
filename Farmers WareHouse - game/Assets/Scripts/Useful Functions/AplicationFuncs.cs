/// <summary>
/// Useful Funcions to manage the core application
/// </summary>
public class AplicationFuncs
{
    /// <summary>
    /// Close the aplication for any state
    /// </summary>
    public static void CloseApp()
    {
        // On unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
}
