using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class MouseWheelZoom : MonoBehaviour
    {
        [SerializeField] private PhotoViewer _viewer;
        [SerializeField] private float _mouseWheelSpeed = 15f;
        private float _mouseWheelDelta;

        private void Update()
        {
            _mouseWheelDelta = Input.GetAxis("Mouse ScrollWheel");
            _mouseWheelDelta = _mouseWheelDelta * Time.deltaTime * _mouseWheelSpeed;

            _viewer.SetZoomSlider(_mouseWheelDelta);
        }
    }
}
