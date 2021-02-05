using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public class PhotoView : MonoBehaviour //, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;

        private RectTransform _transform;
        private RectTransform _imageTransform;
       
        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _imageTransform = _image.GetComponent<RectTransform>();
        }

        public void ShowImage(Sprite sprite)
        {
            _image.sprite = sprite;
            RescalePhoto(sprite);
        }

        private void RescalePhoto(Sprite sprite)
        {
            var rect = _transform.rect;
            Vector2 viewerSize = new Vector2(rect.width, rect.height);
            Vector2 spriteSize = new Vector2(sprite.rect.width,sprite.rect.height);

            float viewerAspect = viewerSize.x / viewerSize.y;
            float spriteAspect = spriteSize.x /spriteSize.y;

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
        }

        public void Clear()
        {
            //  throw new NotImplementedException();
        }
    }
}