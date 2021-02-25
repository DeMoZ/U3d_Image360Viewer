using UnityEngine;
using UnityEngine.EventSystems;

namespace PhotoViewer.Scripts
{
    public abstract class ViewerInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
#if UNITY_IOS || UNITY_ANDROID
        private float _speed = 5;
#else
        private float _speed = 25;
#endif

        protected abstract void OnUpdate();
        
        [SerializeField] protected float _dumping = 3f;
        
        protected Vector2 _deltaPosition = Vector2.zero;

        private bool isToched;

        private void Update()
        {
            if (!isToched)
                _deltaPosition = Vector2.Lerp(_deltaPosition, Vector2.zero, Time.deltaTime * _dumping);

            OnUpdate();
        }

        public void OnBeginDrag(PointerEventData eventData) =>
            isToched = true;

        public void OnDrag(PointerEventData eventData) =>
            _deltaPosition = eventData.delta * Time.deltaTime * _speed;

        public void OnEndDrag(PointerEventData eventData) =>
            isToched = false;

        public void Clear() =>
            _deltaPosition = Vector2.zero;
    }
}