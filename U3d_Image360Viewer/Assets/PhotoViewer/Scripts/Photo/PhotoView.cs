using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts.Photo
{
    public class PhotoView : MonoBehaviour, IPhotoView //, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private PhotoMap _photoMap;

        private RectTransform _transform;
        private RectTransform _imageTransform;
        private Vector2? _defaultImageSize;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _imageTransform = _image.GetComponent<RectTransform>();
        }

        public void ShowImage(Sprite sprite)
        {
            Clear();
            _image.sprite = sprite;
            _photoMap.Clear();
            RescalePhoto(sprite);
            _photoMap.SetMap(_imageTransform.sizeDelta,
                _transform.parent.GetComponent<RectTransform>().sizeDelta + _transform.sizeDelta);
        }

        public void Zoom(float value)
        {
            if (_defaultImageSize == null)
                return;

            _imageTransform.sizeDelta = (Vector2) (_defaultImageSize + _defaultImageSize * value * 10);
            _photoMap.SetMap(_imageTransform.sizeDelta,
                _transform.parent.GetComponent<RectTransform>().sizeDelta + _transform.sizeDelta);
        }

        public void RotateLeft()
        {
              var euler = _imageTransform.rotation.eulerAngles;
              _imageTransform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z + 90);
        }

        public void RotateRigth()
        {
            var euler = _imageTransform.rotation.eulerAngles;
            _imageTransform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z - 90);
        }

        private void RescalePhoto(Sprite sprite)
        {
            var rect = _transform.rect;
            Vector2 viewerSize = new Vector2(rect.width, rect.height);
            Vector2 spriteSize = new Vector2(sprite.rect.width, sprite.rect.height);

            float viewerAspect = viewerSize.x / viewerSize.y;
            float spriteAspect = spriteSize.x / spriteSize.y;

            if (spriteAspect > viewerAspect)
            {
                float relation = viewerSize.x / sprite.texture.width;
                _imageTransform.sizeDelta = new Vector2(viewerSize.x, relation * spriteSize.y);
            }
            else
            {
                float relate = viewerSize.y / sprite.texture.height;
                _imageTransform.sizeDelta = new Vector2(relate * spriteSize.x, viewerSize.y);
            }

            _defaultImageSize = _imageTransform.sizeDelta;
        }

        public void Clear()
        {
            var euler = _imageTransform.rotation.eulerAngles;
            _imageTransform.rotation = Quaternion.Euler(euler.x, euler.y, 0);
        }
    }
}