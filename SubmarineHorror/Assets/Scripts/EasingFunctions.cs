using UnityEngine;

public static class EasingFunctions
{
    public delegate float EasingDelegate(float t);

    public enum FunctionType
    {
        Linear,
        EaseInOutSine,
        EaseInOutBack,
        EaseOutSine,
        EaseOutCirc,
        EaseOutQuart
    }

    public static EasingDelegate DetermineFunction(FunctionType type)
    {
        return type switch
        {
            FunctionType.Linear => Linear,
            FunctionType.EaseInOutSine => EaseInOutSine,
            FunctionType.EaseInOutBack => EaseInOutBack,
            FunctionType.EaseOutSine => EaseOutSine,
            FunctionType.EaseOutCirc => EaseOutCirc,
            FunctionType.EaseOutQuart => EaseOutQuart,
            _ => Linear,
        };
    }

    /// <summary>
    /// Linear function
    /// </summary>
    /// <param name="t">Time [0, 1]</param>
    public static float Linear(float t) => Mathf.Clamp01(t);

    /// <summary>
    /// Easing-in-out sine function
    /// </summary>
    /// <param name="t">Time [0, 1]</param>
    public static float EaseInOutSine(float t) => -(Mathf.Cos(Mathf.PI * Mathf.Clamp01(t)) - 1f) / 2f;

    /// <summary>
    /// Easing-in-out back function
    /// </summary>
    /// <param name="t">Time [0, 1]</param>
    public static float EaseInOutBack(float t)
    {
        t = Mathf.Clamp01(t);
        const float F1 = 1.70158f;
        const float F2 = F1 * 1.525f;

        return t < 0.5f
        ? Mathf.Pow(2f * t, 2f) * ((F2 + 1) * 2f * t - F2) / 2f
        : (Mathf.Pow(2f * t - 2f, 2f) * ((F2 + 1) * (t * 2f - 2f) + F2) + 2f) / 2f;
    }

    /// <summary>
    /// Easing-out sine function
    /// </summary>
    /// <param name="t">Time [0, 1]</param>
    public static float EaseOutSine(float t) => Mathf.Sin(Mathf.Clamp01(t) * Mathf.PI / 2f);

    /// <summary>
    /// Easing-out circ function
    /// </summary>
    /// <param name="t">Time [0, 1]</param>
    public static float EaseOutCirc(float t) => Mathf.Sqrt(1f - ((Mathf.Clamp01(t) - 1) * (Mathf.Clamp01(t) - 1)));

    /// <summary>
    /// Easing-out quart function
    /// </summary>
    /// <param name="t">Time [0, 1]</param>
    public static float EaseOutQuart(float t) => 1f - Mathf.Pow(1f - Mathf.Clamp01(t), 4f);
}
