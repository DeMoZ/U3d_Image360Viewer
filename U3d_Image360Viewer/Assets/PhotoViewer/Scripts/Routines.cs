using System;
using System.Collections;
using System.Timers;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class Routines : MonoBehaviour
    {
       private Coroutine _lerpFloat;

        private float? _current = null;

        public void LerpFloat(float from, float to,float time, bool fromCurrent = false, Action<float> callbackOnLerp = null, Action callbackOnEnd = null)
        {
            if (fromCurrent && _current != null)
                from = (float) _current;

            if (_lerpFloat != null)
            {
                StopCoroutine(_lerpFloat);
                _lerpFloat = null;
            }

            _lerpFloat = StartCoroutine(LerpRoutine(from, to,time, callbackOnLerp, callbackOnEnd));
        }

        private IEnumerator LerpRoutine(float from, float to, float time, Action<float> callbackOnLerp, Action callbackOnEnd)
        {
            float timer = 0;
            
            while (timer< time)
            {
                yield return null;
                
                timer += Time.deltaTime;
               _current= Mathf.Lerp(from, to, timer / time);
                
                callbackOnLerp?.Invoke((float) _current);
            }
 
            yield return null;
            callbackOnEnd?.Invoke();
            _current = null;
            _lerpFloat = null;
        }
    }
}