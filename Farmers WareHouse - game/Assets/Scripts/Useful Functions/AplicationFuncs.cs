using UnityEngine;

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
    /// <summary>
    /// Calculate the resized rect based on proportion
    /// </summary>
    /// <param name="rect">Rect to be transformed</param>
    /// <param name="proportion">Proportions</param>
    /// <returns>Resized Rect</returns>
    public static Rect ResizedRect(Rect canvasSizeDelta, Rect safeArea)
    {
        // TODO FIX fix converter
        return new Rect(rect.position, new Vector2(rect.width * proportion.x, rect.height * proportion.y));
    }
}
