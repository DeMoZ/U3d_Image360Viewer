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
        private PanoramaRotator sphere;
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
            _sprite = sprite;
            material.mainTexture = sprite.TextureFromSprite();

            if (_animateIcon360)
                AnimateIcon360();
            // RescalePanorama();
        }

        public void ApplyInput(Vector2 deltaPosition)
        {
            OnRotate?.Invoke(deltaPosition.x);
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

        private void RescalePanorama() =>
            GetComponent<RectTransform>().sizeDelta = new Vector2(1024, 768);

        private void AnimateIcon360()
        {
            var color = _icon360.color;
            color.a = 0;
            _icon360.color = color;
            _icon360T.localScale = Vector3.one * 5f;

            _icon360T.anchoredPosition = _localCenter;

            _routines.LerpFloat(0, _iconColor.a, 0.3f, false, (a) =>
                {
                    color.a = a;
                    _icon360.color = color;
                },
                () =>
                {
                    _routines.LerpVector2(_icon360T.anchoredPosition, _iconPosition, 0.5f, false,
                        (vector) => { _icon360T.anchoredPosition = vector; });
                    
                   // _routines.LerpVector2(_icon360T.localScale, Vector2.one, 0.5f, false,
                    //    (vector) => { _icon360T.localScale = vector; });
                });
        }

        private void _AnimateIcon360()
        {
            if (_iconRoutine != null)
            {
                StopCoroutine(_iconRoutine);
                _iconRoutine = null;
            }

            _iconRoutine = StartCoroutine(IconRoutine());
        }

        private IEnumerator IconRoutine()
        {
            var animateTimer = 0f;
            _icon360T.anchoredPosition = _localCenter;

            while (animateTimer < _iconAnimateTime)
            {
                yield return null;

                animateTimer += Time.deltaTime;

                var position = Vector2.Lerp(_icon360T.anchoredPosition, _iconPosition,
                    animateTimer / _iconAnimateTime);

                _icon360T.anchoredPosition = position;
            }

            _icon360T.anchoredPosition = _iconPosition;

            _iconRoutine = null;
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

        private void InstantiateObjects()
        {
            Transform groupParent = new GameObject().transform;
            groupParent.position = Vector3.one * 100;
            groupParent.name = "PanoramaGroup";

            Instantiate(_panoramaCameraPrefab, groupParent).GetComponent<Camera>();
            sphere = Instantiate(_panoramaSpherePrefab, groupParent);

            material = sphere.GetComponent<Renderer>().material;
            OnRotate += sphere.OnRotate;
        }

        private void _InstantiateObjects()
        {
            Transform groupParent = new GameObject().transform;
            groupParent.position = Vector3.one * 100;
            groupParent.name = "PanoramaGroup";

            Instantiate(_panoramaCameraPrefab, groupParent);
            sphere = Instantiate(_panoramaSpherePrefab, groupParent);

            material = sphere.GetComponent<Renderer>().material;
            OnRotate += sphere.OnRotate;
        }

        private void OnDestroy() =>
            OnRotate -= sphere.OnRotate;


        public void Zoom(float value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            // throw new NotImplementedException();
        }
    }
}