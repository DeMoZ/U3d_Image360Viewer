using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts.Panorama
{
    [RequireComponent(typeof(Routines))]
    public class Panorama : AbstractView
    {
        private const float IconAnimateTime = 0.5f;

        [SerializeField] private PanoramaRotator _panoramaCameraPrefab = null;
        [SerializeField] private GameObject _panoramaSpherePrefab = null;
        [SerializeField] private PanoramaMap _panoramaMap = null;
        [SerializeField] private Image _icon360 = null;

        [Tooltip("When true, the 360 icon will appear in center if view and animate to a default position.")]
        [SerializeField]
        private bool _animateIcon360 = true;

        public event Action<Vector2> OnRotate;

        private Routines _routines;
        private RectTransform _icon360T;
        private GameObject _sphere;
        private PanoramaRotator _cameraR;
        private Camera _camera;
        private Material _material;
        private Vector2 _localCenter;
        private Vector2 _iconPosition;
        private Color _iconColor;

        protected override void Awake()
        {
            _routines = GetComponent<Routines>();
            SetIcon();
            InstantiateObjects();

            OnRotate += _cameraR.OnRotate;
            OnRotate += _panoramaMap.OnRotate;

            base.Awake();
        }

        protected override void ShowData(ImageData imageData)
        {
            _zoomSlider.onValueChanged.AddListener(Zoom);
            _btnReset.Show(false);

            _material.mainTexture = imageData.Sprite.TextureFromSprite();

            if (_animateIcon360)
                AnimateIcon360();
        }

        public override void Clear()
        {
            _cameraR.transform.rotation = Quaternion.identity;
            _panoramaMap.Clear();

            _zoomSlider.value = 0.5f;
            Zoom(0.5f);
        }

        public override void ApplyInput(Vector2 deltaPosition)
        {
            OnRotate?.Invoke(deltaPosition);

            if (deltaPosition != Vector2.zero)
                OnChange?.Invoke();
        }

        protected override void ShowMap(bool show) =>
            _panoramaMap.Show(show);

        protected override void Zoom(float value)
        {
            OnChange?.Invoke();
            ShowMap(true);

            _camera.fieldOfView = value * 123;
            _cameraR.SetClamp(89 - _camera.fieldOfView / 2);
            _panoramaMap.SetViewPort(_camera.fieldOfView);
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
                    _routines.LerpVector2(_icon360T.anchoredPosition, _iconPosition, IconAnimateTime,
                        (vector) => { _icon360T.anchoredPosition = vector; });

                    _routines.LerpVector2(_icon360T.localScale, Vector2.one, IconAnimateTime,
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

            _material = _sphere.GetComponent<Renderer>().material;
        }

        private new void OnDestroy()
        {
            OnRotate -= _cameraR.OnRotate;
            OnRotate -= _panoramaMap.OnRotate;
        }
    }
}