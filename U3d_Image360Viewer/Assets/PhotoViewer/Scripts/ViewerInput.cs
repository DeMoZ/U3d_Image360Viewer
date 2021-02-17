using UnityEngine;
using UnityEngine.EventSystems;

namespace PhotoViewer.Scripts
{
    public abstract class ViewerInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected abstract void OnUpdate();

        [SerializeField] protected float _dumping = 5f;
        [SerializeField] private float _speedMobile = 5f;
        [SerializeField] private float _speedOther = 25f;

        protected Vector2 _deltaPosition = Vector2.zero;
        private float _speedCurrent;
        private bool isToched;

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
            if (!isToched)
                _deltaPosition = Vector2.Lerp(_deltaPosition, Vector2.zero, Time.deltaTime * _dumping);

            OnUpdate();
        }

        public void OnBeginDrag(PointerEventData eventData) =>
            isToched = true;
              
        public void OnDrag(PointerEventData eventData) =>
            _deltaPosition = eventData.delta * Time.deltaTime * _speedCurrent;

        public void OnEndDrag(PointerEventData eventData) =>
            isToched = false;
    }
}