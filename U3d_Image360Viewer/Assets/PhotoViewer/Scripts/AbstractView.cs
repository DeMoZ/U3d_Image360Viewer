using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public abstract class AbstractView : MonoBehaviour
    {
        [SerializeField] protected Text _name = default;
        [SerializeField] protected Slider _zoomSlider = default;
        [SerializeField] protected ResetButton _btnReset = default;
        [SerializeField] protected Text _date = default;
 
        public event Action OnShow;
        public abstract void Zoom(float value);

        public abstract void ApplyInput(Vector2 deltaPosition);

        protected Action OnChange;
        protected abstract void ShowMap(bool show);

        protected abstract void ShowData(ImageData imageData);

        public Slider ZoomSlider  { 
            get=>_zoomSlider;
            set=>_zoomSlider=value; 
        }
        
        public void Show(ImageData imageData)
        {
            ShowData(imageData);
            ShowMap(false);
            _btnReset.Show(false);
            
            OnShow?.Invoke();
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