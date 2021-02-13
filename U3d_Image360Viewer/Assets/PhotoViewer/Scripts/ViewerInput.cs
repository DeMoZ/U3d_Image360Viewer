using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhotoViewer.Scripts
{
    public abstract class ViewerInput : MonoBehaviour, IDragHandler, IBeginDragHandler , IPointerDownHandler
    {
        protected abstract void OnUpdate();

        [SerializeField] protected float _dumping = 5f;
        [SerializeField] private float _speedMobile = 5f;
        [SerializeField] private float _speedOther = 25f;

        protected Vector2 _deltaPosition = Vector2.zero;

        private float _speedCurrent;

        private void Awake() => 
            SetSpeed();

        private void SetSpeed()
        {
#if UNITY_IOS || UNITY_ANDROID
            _speedCurrent = _speedMobile;
#else
            _speedCurrent = _speedOther;
#endif
        }

        private void Update()
        {
            _deltaPosition = Vector2.Lerp(_deltaPosition, Vector2.zero, Time.deltaTime * _dumping);

            OnUpdate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData) => 
            _deltaPosition = eventData.delta * Time.deltaTime * _speedCurrent;

        public void OnDrag(PointerEventData eventData) =>
            _deltaPosition = eventData.delta * Time.deltaTime * _speedCurrent;
    }
}