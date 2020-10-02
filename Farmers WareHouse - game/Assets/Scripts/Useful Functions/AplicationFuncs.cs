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
    /// Calculate the screen position on canvas position
    /// </summary>
    /// <param name= "ScreenPos">Screen position to be calculated</param>
    /// <param name="canvasRect">Canvas Rect</param>
    /// <returns>The position on Canvas</returns>
    public static Vector2 ScreenToCanvasPos(Vector2 ScreenPos, Rect canvasRect)
    {
        return new Vector2();
    }

    /// <summary>
    /// Calculate the resized rect 
    /// </summary>
    /// <param name="canvasRect">Rect to be transformed</param>
    /// <returns>Resized Rect</returns>
    public static Rect SafeToSafeCanvas(Rect canvasRect)
    {
        // should calculate the safe area size on canvas
        return new Rect
        {
            // Set the Rect min position
            min = AplicationFuncs.ScreenToCanvasPos(ScreenPos: Screen.safeArea.min, canvasRect: canvasRect),
            // Set the max postion 
            max = AplicationFuncs.ScreenToCanvasPos(ScreenPos: Screen.safeArea.max, canvasRect: canvasRect)
        };
    }
}
