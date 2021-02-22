using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class MouseWheelZoom : MonoBehaviour
    {
        [SerializeField] private PhotoViewer _viewer = null;
        [SerializeField] private float _speed = 25f;
        [SerializeField] private float _dumping = 4f;

        private float _delta;

        private void Start() => 
            _viewer.SubscribeMeOnNewImage(Clear);

        private void Update()
        {
            var delta = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * _speed;
            _delta = Mathf.Lerp(_delta, delta, Time.deltaTime * _dumping);
            _viewer.SetZoomSlider(_delta);
        }

        private void Clear() => 
            _delta = 0;

        private void OnDestroy()=>
            _viewer.UnSubscribeMeOnNewImage(Clear);
    }
}
