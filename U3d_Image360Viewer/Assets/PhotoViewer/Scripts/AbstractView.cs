using System;
using PhotoViewer.Scripts.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public abstract class AbstractView : MonoBehaviour
    {
        [SerializeField] protected Text _name = default;
        [SerializeField] protected Text _date = default;
        [SerializeField] protected Slider _zoomSlider = default;
        [SerializeField] protected ResetButton _btnReset = default;

        public event Action OnShow;

        protected Action OnChange;

        private ImageData _currentData;

        public abstract void ApplyInput(Vector2 deltaPosition);
        public abstract void Clear();

        protected abstract void Zoom(float value);
        protected abstract void ShowMap(bool show);
        protected abstract void ShowData(ImageData imageData);

        protected virtual void Awake()
        {
            OnChange += () => { ShowMap(true); };
            OnChange += () => { _btnReset.Show(true); };
        }

        public Slider ZoomSlider
        {
            get => _zoomSlider;
            set => _zoomSlider = value;
        }

        public void Show(ImageData imageData)
        {
            Clear();
            _currentData = imageData;
            _name.text = imageData.Name;
            _date.text = imageData.Date;
            ShowData(imageData);
            ShowMap(false);
            _btnReset.Show(false);

            OnShow?.Invoke();
        }

        public void Reset()
        {
            Clear();
            Show(_currentData);
        }

        protected void ResetZoom(float value = 0)
        {
            _zoomSlider.onValueChanged.RemoveListener(Zoom);
            _zoomSlider.value = value;
            _zoomSlider.onValueChanged.AddListener(Zoom);
        }

        protected void OnDestroy() =>
            _zoomSlider.onValueChanged.RemoveAllListeners();
    }
}