using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static IEnumerator CrossFading<A>(A from, A to, float duration, Action<A> setter, Func<A, A, float, A> lerp)
    {
        float timer = 0f;

        while (timer <= duration)
        {
            timer += Time.unscaledDeltaTime;
            setter(lerp(from, to, timer / duration));
            yield return null;
        }
    }

    public static IEnumerator DelayedCall(float delay, Action call)
    {
        yield return new WaitForSeconds(delay);
        call.Invoke();
    }

    public static IEnumerator DelayedCrossFading<A>(float delay, A from, A to, float duration, Action<A> setter, Func<A, A, float, A> lerp)
    {
        yield return new WaitForSeconds(delay);

        float timer = 0f;

        while (timer <= duration)
        {
            timer += Time.unscaledDeltaTime;
            setter(lerp(from, to, timer / duration));
            yield return null;
        }
    }
}
