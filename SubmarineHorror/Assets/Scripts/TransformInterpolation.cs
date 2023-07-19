using System.Collections;
using UnityEngine;

public static class TransformInterpolation
{
    /// <summary>
    /// Interpolate towards a position
    /// </summary>
    /// <param name="to">The position to move towards</param>
    /// <param name="duration">The time to move towards "to" position</param>
    /// <param name="easeType">The type of interpolation to perform</param>
    public static IEnumerator MoveTo(this Transform transform, Vector3 to, float duration, EasingFunctions.FunctionType easeType)
    {
        if (duration < 0f) yield break;

        EasingFunctions.EasingDelegate easingDelegate = EasingFunctions.DetermineFunction(easeType);
        Vector3 startingPos = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            Vector3 lerped = Vector3.LerpUnclamped(startingPos, to, easingDelegate.Invoke(elapsed / duration));
            transform.position = lerped;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        transform.position = to;
    }

    /// <summary>
    /// Interpolate towards a position, then interpolate back to the origin position
    /// </summary>
    /// <param name="to">The position to move towards</param>
    /// <param name="moveDuration">How long it takes to move towards the target position, as well as to move back</param>
    /// <param name="waitDuration">How long to wait before moving back (after reaching target position)</param>
    /// <param name="easeType">The type of interpolation to perform</param>
    public static IEnumerator MoveToReturn(this Transform transform, Vector3 to, float moveDuration, float waitDuration, EasingFunctions.FunctionType easeType)
    {
        if (moveDuration < 0f || waitDuration < 0f) yield break;

        Vector3 startingPos = transform.position;

        CoroutineManager.Start(MoveTo(transform, to, moveDuration, easeType));
        // Waiting until reaching goal position
        yield return new WaitUntil(() => transform.position == to);
        yield return new WaitForSeconds(waitDuration);
        CoroutineManager.Start(MoveTo(transform, startingPos, moveDuration, easeType));
    }

    /// <summary>
    /// Rotate towards a vector
    /// </summary>
    /// <param name="to">The vector to rotate towards</param>
    /// <param name="duration">The time to rotate towards "to" vector</param>
    /// <param name="easeType">The type of interpolation to perform</param>
    public static IEnumerator RotateTo(this Transform transform, Vector3 to, float duration, EasingFunctions.FunctionType easeType)
    {
        if (duration < 0f) yield break;

        EasingFunctions.EasingDelegate easingDelegate = EasingFunctions.DetermineFunction(easeType);
        Quaternion toRot = Quaternion.Euler(to), startingRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Quaternion lerped = Quaternion.LerpUnclamped(startingRot, toRot, easingDelegate.Invoke(elapsed / duration));
            transform.rotation = lerped;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = toRot;
    }

    /// <summary>
    /// Scale towards a vector
    /// </summary>
    /// <param name="to">The vector to scale towards</param>
    /// <param name="duration">The time to scale towards "to" vector</param>
    /// <param name="easeType">The type of interpolation to perform</param>
    public static IEnumerator ScaleTo(this Transform transform, Vector3 to, float duration, EasingFunctions.FunctionType easeType)
    {
        if (duration < 0f) yield break;

        EasingFunctions.EasingDelegate easingDelegate = EasingFunctions.DetermineFunction(easeType);
        Vector3 startingScale = transform.localScale;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            Vector3 lerped = Vector3.LerpUnclamped(startingScale, to, easingDelegate.Invoke(elapsed / duration));
            transform.localScale = lerped;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = to;
    }
}
