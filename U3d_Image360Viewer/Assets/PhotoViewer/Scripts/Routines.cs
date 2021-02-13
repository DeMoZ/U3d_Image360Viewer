using System;
using System.Collections;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class Routines : MonoBehaviour
    {
       private Coroutine _lerpFloat;

        private float? _currentFloat = null;
        private Vector2? _currentVector2 = null;

        public void LerpFloat(float from, float to,float time, bool fromCurrent = false, Action<float> callbackOnLerp = null, Action callbackOnEnd = null)
        {
            if (fromCurrent && _currentFloat != null)
                from = (float) _currentFloat;

            if (_lerpFloat != null)
            {
                StopCoroutine(_lerpFloat);
                _lerpFloat = null;
            }

            _lerpFloat = StartCoroutine(LerpRoutineFloat(from, to,time, callbackOnLerp, callbackOnEnd));
        }

        public void LerpVector2(Vector2 from, Vector2 to,float time, bool fromCurrent = false, Action<Vector2> callbackOnLerp = null, Action callbackOnEnd = null)
        {
            if (fromCurrent && _currentVector2 != null)
                from = (Vector2)_currentVector2 ;

            if (_lerpFloat != null)
            {
                StopCoroutine(_lerpFloat);
                _lerpFloat = null;
            }

            _lerpFloat = StartCoroutine(LerpRoutineVector2(from, to,time, callbackOnLerp, callbackOnEnd));
        }
        
        private IEnumerator LerpRoutineVector2(Vector2 from, Vector2 to, float time, Action<Vector2> callbackOnLerp, Action callbackOnEnd)
        {
            float timer = 0;
            
            while (timer< time)
            {
                yield return null;
                
                timer += Time.deltaTime;
                _currentVector2= Vector2.Lerp(from, to, timer / time);
                
                callbackOnLerp?.Invoke((Vector2) _currentVector2);
            }
 
            yield return null;
            callbackOnEnd?.Invoke();
            _currentFloat = null;
            _lerpFloat = null;
        }
        
        private IEnumerator LerpRoutineFloat(float from, float to, float time, Action<float> callbackOnLerp, Action callbackOnEnd)
        {
            float timer = 0;
            
            while (timer< time)
            {
                yield return null;
                
                timer += Time.deltaTime;
               _currentFloat= Mathf.Lerp(from, to, timer / time);
                
                callbackOnLerp?.Invoke((float) _currentFloat);
            }
 
            yield return null;
            callbackOnEnd?.Invoke();
            _currentFloat = null;
            _lerpFloat = null;
        }
    }
}