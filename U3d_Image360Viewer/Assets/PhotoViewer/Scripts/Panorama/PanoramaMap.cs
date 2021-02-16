using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts.Panorama
{
    public class PanoramaMap : MonoBehaviour
    {
        [SerializeField] private RectTransform _map;
        [SerializeField] private Image _viewPort;
        private RectTransform _viewPortT;

        private void Awake()
        {
            _viewPortT = _viewPort.GetComponent<RectTransform>();
        }

        public void Clear()
        {
        }

        public void OnRotate(Vector2 deltaRotation)
        {
            var rotation = _viewPortT.rotation.eulerAngles + Vector3.forward * deltaRotation.x;
            _viewPortT.rotation = Quaternion.Euler(rotation);
        }

        public void SetViewPort(float value) =>
            _viewPort.fillAmount = value / 360;
    }
}