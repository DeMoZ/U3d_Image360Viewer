using System;
using UnityEngine;
using System.Collections;

namespace PhotoViewer.Scripts
{
    public class Routines : MonoBehaviour
    {
        public void StopAllRoutines()
        {
            StopAllCoroutines();
        }
        
        // public void NewRoutine<T>(T from, T to, float time, Action<T> callbackOnLerp = null,
        //     Action callbackOnEnd = null) where T : struct
        // {
        //     if (typeof(T) == typeof(float))
        //         StartCoroutine(LerpRoutineFloat((float) (object) from, (float) (object) to, time,
        //             (Action<float>) (object) callbackOnLerp, callbackOnEnd));
        //     
        //     else if (typeof(T) == typeof(Vector2))
        //         StartCoroutine(LerpRoutineVector2((Vector2) (object) from, (Vector2) (object) to, time,
        //             (Action<Vector2>) (object) callbackOnLerp, callbackOnEnd));
        // }

        public void LerpFloat(float from, float to, float time, Action<float> callbackOnLerp = null,
            Action callbackOnEnd = null) =>
            StartCoroutine(LerpRoutineFloat(@from, to, time, callbackOnLerp, callbackOnEnd));

        public void LerpVector2(Vector2 from, Vector2 to, float time, Action<Vector2> callbackOnLerp = null,
            Action callbackOnEnd = null) =>
            StartCoroutine(LerpRoutineVector2(@from, to, time, callbackOnLerp, callbackOnEnd));

        private IEnumerator LerpRoutineVector2(Vector2 from, Vector2 to, float time, Action<Vector2> callbackOnLerp,
            Action callbackOnEnd)
        {
            var value = from;
            float timer = 0;

            while (timer < time)
            {
                yield return null;

                timer += Time.deltaTime;
                value = Vector2.Lerp(from, to, timer / time);

                callbackOnLerp?.Invoke(value);
            }

            callbackOnEnd?.Invoke();
        }

        private IEnumerator LerpRoutineFloat(float from, float to, float time, Action<float> callbackOnLerp,
            Action callbackOnEnd)
        {
            var value = from;
            float timer = 0;

            while (timer < time)
            {
                yield return null;

                timer += Time.deltaTime;
                value = Mathf.Lerp(from, to, timer / time);

                callbackOnLerp?.Invoke(value);
            }

            callbackOnEnd?.Invoke();
        }
    }
}