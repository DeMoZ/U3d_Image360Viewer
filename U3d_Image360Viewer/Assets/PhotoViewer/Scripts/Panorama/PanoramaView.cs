using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts.Panorama
{
    [RequireComponent(typeof(Routines))]
    public class PanoramaView : MonoBehaviour, IPhotoView
    {
        [SerializeField] private PanoramaRotator _panoramaCameraPrefab;
        [SerializeField] private GameObject _panoramaSpherePrefab;
        [SerializeField] private PanoramaMap _panoramaMap;
        [SerializeField] private Image _icon360;

        [Tooltip("When true, the 360 icon will appear in center if view and animate to a default position.")]
        [SerializeField]
        private bool _animateIcon360 = true;

        private Routines _routines;

        private Sprite _sprite;

        private RenderTexture _renderTexture;
        private RectTransform _icon360T;
        private GameObject _sphere;
        private PanoramaRotator _cameraR;
        private Camera _camera;
        private Material material;

        private float rotationSpeed;

        public event Action<Vector2> OnRotate;
        private float screenWidth;
        private Direction beginDragDirection;
        private Vector2 _localCenter;

        private Coroutine _iconRoutine;
        private Vector2 _iconPosition;
        private float _iconAnimateTime = 1f;
        private Color _iconColor;

        private event Action OnChange;
        private enum Direction
        {
            Horizontal,
            Vertical,
        }

        private void Awake()
        {
            _renderTexture = GetComponent<RenderTexture>();
            _routines = GetComponent<Routines>();

            SetIcon();
            InstantiateObjects();

            OnRotate += _cameraR.OnRotate;
            OnRotate += _panoramaMap.OnRotate;
        }

        public void ShowImage(Sprite sprite)
        {
            Clear();
            _sprite = sprite;
            material.mainTexture = sprite.TextureFromSprite();

            if (_animateIcon360)
                AnimateIcon360();
        }

        public void Clear()
        {
            _cameraR.transform.rotation = Quaternion.Euler(Vector3.zero);
            _panoramaMap.Clear();
            Zoom(0.5f);
        }

        public void ApplyInput(Vector2 deltaPosition)
        {
            OnRotate?.Invoke(deltaPosition);

            if (deltaPosition != Vector2.zero)
                OnChange?.Invoke();
        }

        private void SetIcon()
        {
            _iconColor = _icon360.color;
            _icon360T = _icon360.GetComponent<RectTransform>();
            _iconPosition = _icon360T.anchoredPosition;
            var x = GetComponent<RectTransform>().rect.width / 2;
            var y = -GetComponent<RectTransform>().rect.height / 2;
            _localCenter = new Vector2(x, y);
        }

        public void Zoom(float value)
        {
            _camera.fieldOfView = value * 123;
            _panoramaMap.SetViewPort(_camera.fieldOfView);
        }

        public void SubscribeMeOnChange(Action callback) =>
            OnChange += callback;

        public void UnSubscribeMeOnChange(Action callback) =>
            OnChange -= callback;

        private void AnimateIcon360()
        {
            var color = _icon360.color;
            color.a = 0;
            _icon360.color = color;
            _icon360T.localScale = Vector3.one * 5f;

            _icon360T.anchoredPosition = _localCenter;

            _routines.LerpFloat(0, _iconColor.a, 0.3f, (a) =>
                {
                    color.a = a;
                    _icon360.color = color;
                },
                () =>
                {
                    _routines.LerpVector2(_icon360T.anchoredPosition, _iconPosition, 0.5f,
                        (vector) => { _icon360T.anchoredPosition = vector; });

                    _routines.LerpVector2(_icon360T.localScale, Vector2.one, 0.5f,
                        (vector) => { _icon360T.localScale = vector; });
                });
        }

        private void InstantiateObjects()
        {
            Transform groupParent = new GameObject().transform;
            groupParent.position = Vector3.one * 100;
            groupParent.name = "PanoramaGroup";

            _cameraR = Instantiate(_panoramaCameraPrefab, groupParent);
            _camera = _cameraR.GetComponent<Camera>();
            _sphere = Instantiate(_panoramaSpherePrefab, groupParent);

            material = _sphere.GetComponent<Renderer>().material;
        }

        private void OnDestroy()
        {
            OnRotate -= _cameraR.OnRotate;
            OnRotate -= _panoramaMap.OnRotate;
        }
    }
}