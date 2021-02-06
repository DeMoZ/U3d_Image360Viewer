using System;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class PhotoMap : MonoBehaviour
    {
        [SerializeField] private RectTransform _map;
        [SerializeField] private RectTransform _picture;
        
        private Vector2 _mapDefaultSize = new Vector2();
        private Vector2 _pictureDefaultSize = new Vector2();

        private Vector2 _photoMapSize = new Vector2();
        
        private void Awake()
        {
            _photoMapSize = GetComponent<RectTransform>().sizeDelta;
            _mapDefaultSize = _map.sizeDelta;
            _pictureDefaultSize = _picture.sizeDelta;
        }

        public void Clear()
        {
            _map.sizeDelta = _mapDefaultSize;
            _picture.sizeDelta = _pictureDefaultSize;
        }

        public void SetMap(Vector2 imageSize, Vector2 viewSize)
        {
            //float viewAspect = viewSize.x / viewSize.y;
            float pictureAspect = imageSize.x / imageSize.y;
            float mapAspect = _photoMapSize.x / _photoMapSize.y;
            
            if (pictureAspect > mapAspect)
            {
                float relation = _photoMapSize.x / imageSize.x;
               // _imageTransform.sizeDelta = new Vector2(viewSize.x, relation * imageSize.y);
               _picture.sizeDelta = new Vector2(_photoMapSize.x,relation*imageSize.y);
            }
            else
            {
                float relation = _photoMapSize.y / imageSize.y;
               // _imageTransform.sizeDelta = new Vector2(relate * imageSize.x, viewSize.y);
               _picture.sizeDelta = new Vector2(relation*imageSize.x,_photoMapSize.y);
            }

           // _defaultImageSize = _imageTransform.sizeDelta;
            
        }
    }
}