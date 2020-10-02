using System;

public static class MathHelper
{
    /// <summary>
    /// Remap a  value between one set to other
    /// </summary>
    /// <param name="value">Value To be remaped</param>
    /// <param name="from1">Min from set</param>
    /// <param name="to1">Max to Set</param>
    /// <param name="from2">Min from new set</param>
    /// <param name="to2">Max to new set</param>
    /// <returns>Remaped Value</returns>
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
