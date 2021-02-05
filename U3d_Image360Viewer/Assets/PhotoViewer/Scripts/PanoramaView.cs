using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public class PanoramaView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject panoramaCameraPrefab;
        [SerializeField] private PanoramaRotator panoramaSpherePrefab;
        [SerializeField] private Image icon360;

        [SerializeField] private float rotationSpeedMobile = 5f;
        [SerializeField] private float rotationSpeedOther = 25f;
        [SerializeField] private Direction rotateDirection = Direction.Horizontal;

        public Sprite sprite { get; set; }

        private RenderTexture renderTexture;
        private RectTransform icon360T;
        private PanoramaRotator sphere;
        private Material material;

        private float rotationSpeed;

        public event Action<float> OnRotate;
        private float screenWidth;
        private Direction beginDragDirection;
        private Vector2 iconPosition;
        private Vector2 iconPositionCenter;

        private enum Direction
        {
            Horizontal,
            Vertical,
        }

        private void Awake()
        {
            renderTexture = GetComponent<RenderTexture>();
            icon360T = icon360.GetComponent<RectTransform>();
            SetIconPositions();
            InstantiateObjects();
            SetRotateSpeed();
        }

        public void ShowImage(Sprite sprite)
        {
            this.sprite = sprite;
            material.mainTexture = sprite.TextureFromSprite();

            // RescalePanorama();
        }

        private void RescalePanorama()
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(1024, 768);
        }

//==========================================================================================================
        public void LeanIcon()
        {
            Color c = icon360.color;
            c.a = 0;
            icon360.color = c;
            icon360T.localScale = Vector3.one * 5f;

            icon360T.anchoredPosition = iconPositionCenter;

            //  LeanTween.alpha(icon360T, 0.5f, 0.3f).setOnComplete(() =>
            //  {
            //      LeanTween.scale(icon360T, Vector3.one, 0.5f);
            //      LeanTween.move(icon360T, iconPosition, 0.5f);
            //  });
        }

        private void SetIconPositions()
        {
            iconPosition = icon360T.anchoredPosition;
            float x = GetComponent<RectTransform>().rect.width / 2;
            float y = -GetComponent<RectTransform>().rect.height / 2;
            iconPositionCenter = new Vector2(x, y);
        }

        private void InstantiateObjects()
        {
            Transform groupParent = new GameObject().transform;
            groupParent.position = Vector3.one * 100;
            groupParent.name = "PanoramaGroup";

            Instantiate(panoramaCameraPrefab, groupParent);
            sphere = Instantiate(panoramaSpherePrefab, groupParent);

            material = sphere.GetComponent<Renderer>().material;
            OnRotate += sphere.OnRotate;
        }

        private void SetRotateSpeed()
        {
#if UNITY_IOS || UNITY_ANDROID
            rotationSpeed = rotationSpeedMobile;
#else
            rotationSpeed = rotationSpeedOther;
#endif
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            beginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)
                ? Direction.Horizontal
                : Direction.Vertical;

            if (beginDragDirection != rotateDirection)
                return;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 deltaPosition = eventData.delta * Time.deltaTime * rotationSpeed;
            OnRotate?.Invoke(deltaPosition.x);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        private void OnDestroy() =>
            OnRotate -= sphere.OnRotate;

        public void Clear()
        {
            // throw new NotImplementedException();
        }
    }
}