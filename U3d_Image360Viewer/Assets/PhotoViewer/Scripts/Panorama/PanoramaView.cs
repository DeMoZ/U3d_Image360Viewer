using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace PhotoViewer.Scripts.Panorama
{
    [RequireComponent(typeof(Routines))]
    public class PanoramaView : MonoBehaviour, IPhotoView //, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject _panoramaCameraPrefab;
        [SerializeField] private PanoramaRotator _panoramaSpherePrefab;
        [SerializeField] private Image _icon360;

        [Tooltip("When true, the 360 icon will appear in center if view and animate to a default position.")]
        [SerializeField]
        private bool _animateIcon360 = true;

        private Routines _routines;

        private Sprite _sprite;

        private RenderTexture _renderTexture;
        private RectTransform _icon360T;
        private PanoramaRotator _sphere;
        private Camera _camera;
        private Material material;

        private float rotationSpeed;

        public event Action<float> OnRotate;
        private float screenWidth;
        private Direction beginDragDirection;
        private Vector2 _localCenter;

        private Coroutine _iconRoutine;
        private Vector2 _iconPosition;
        private float _iconAnimateTime = 1f;
        private Color _iconColor;

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
            // SetRotateSpeed();
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
            // throw new NotImplementedException();
        }

        public void ApplyInput(Vector2 deltaPosition) =>
            OnRotate?.Invoke(deltaPosition.x);

        private void SetIcon()
        {
            _iconColor = _icon360.color;
            _icon360T = _icon360.GetComponent<RectTransform>();
            _iconPosition = _icon360T.anchoredPosition;
            var x = GetComponent<RectTransform>().rect.width / 2;
            var y = -GetComponent<RectTransform>().rect.height / 2;
            _localCenter = new Vector2(x, y);
        }

        public void Zoom(float value) => 
            _camera.fieldOfView = value * 123;

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

            _camera = Instantiate(_panoramaCameraPrefab, groupParent).GetComponent<Camera>();
            _sphere = Instantiate(_panoramaSpherePrefab, groupParent);

            material = _sphere.GetComponent<Renderer>().material;
            OnRotate += _sphere.OnRotate;
        }

        //==========================================================================================================


        public void LeanIcon()
        {
            Color c = _icon360.color;
            c.a = 0;
            _icon360.color = c;
            _icon360T.localScale = Vector3.one * 5f;

            _icon360T.anchoredPosition = _localCenter;

            //  LeanTween.alpha(icon360T, 0.5f, 0.3f).setOnComplete(() =>
            //  {
            //      LeanTween.scale(icon360T, Vector3.one, 0.5f);
            //      LeanTween.move(icon360T, iconPosition, 0.5f);
            //  });
        }


        private void OnDestroy() =>
            OnRotate -= _sphere.OnRotate;
    }
}