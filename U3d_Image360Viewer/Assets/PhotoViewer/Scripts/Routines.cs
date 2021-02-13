using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class Routines : MonoBehaviour
    {
        private List<Coroutine> _coroutines = new List<Coroutine>();

        public void LerpFloat(float from, float to, float time, Action<float> callbackOnLerp = null,
            Action callbackOnEnd = null)
        {
            _coroutines.Add(StartCoroutine(LerpRoutineFloat(from, to, time, callbackOnLerp, callbackOnEnd)));
        }

        public void LerpVector2(Vector2 from, Vector2 to, float time, Action<Vector2> callbackOnLerp = null,
            Action callbackOnEnd = null)
        {
            _coroutines.Add(StartCoroutine(LerpRoutineVector2(from, to, time, callbackOnLerp, callbackOnEnd)));
        }

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

            yield return null;
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

            yield return null;
            callbackOnEnd?.Invoke();
        }
    }
}