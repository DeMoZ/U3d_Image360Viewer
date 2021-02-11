using UnityEngine;

namespace PhotoViewer.Scripts.Photo
{
    public class PhotoMap : MonoBehaviour
    {
        [SerializeField] private RectTransform _map;
        [SerializeField] private RectTransform _picture;

        private Vector2 _photoMapSize = new Vector2();

        private void Awake() =>
            _photoMapSize = GetComponent<RectTransform>().sizeDelta;

        public void Clear()
        {
        }

        public void SetPosition(Vector2 position, Vector2 imgSize)
        {
            var x = -position.x * (_photoMapSize.x / imgSize.x) + _photoMapSize.x / 2;
            var y = -position.y * (_photoMapSize.y / imgSize.y) + _photoMapSize.y / 2;

            _map.localPosition = new Vector2(x, y);
        }

        public void SetSize(Vector2 imageSize, Vector2 viewSize)
        {
            var imageMagn = imageSize.magnitude;
            var viewMagn = viewSize.magnitude;

            Debug.Log($"imageSize {imageSize}, viewSize {viewSize}");
            Debug.Log($"imageMagn {imageMagn}, viewMagn {viewMagn}");

            var bigElementSize = (imageMagn > viewMagn) ? imageSize : viewSize;
            var bigElementRect = (imageMagn > viewMagn) ? _picture : _map;

            var bigAspect = bigElementSize.x / bigElementSize.y;
            var mapAspect = _photoMapSize.x / _photoMapSize.y;

            if (bigAspect > mapAspect)
            {
                var relation = _photoMapSize.x / bigElementSize.x;
                bigElementRect.sizeDelta = new Vector2(_photoMapSize.x, relation * bigElementSize.y);
            }
            else
            {
                var relation = _photoMapSize.y / bigElementSize.y;
                bigElementRect.sizeDelta = new Vector2(relation * bigElementSize.x, _photoMapSize.y);
            }

            InnerPart(bigElementSize, viewSize);
        }

        public void __SetMap(Vector2 imageSize, Vector2 viewSize)
        {
            var imageMagn = imageSize.magnitude;
            var viewMagn = viewSize.magnitude;

            Debug.Log($"imageSize {imageSize}, viewSize {viewSize}");
            Debug.Log($"imageMagn {imageMagn}, viewMagn {viewMagn}");

            var bigElementSize = (imageMagn > viewMagn) ? imageSize : viewSize;
            var bigElementRect = (imageMagn > viewMagn) ? _picture : _map;

            var bigAspect = bigElementSize.x / bigElementSize.y;
            var mapAspect = _photoMapSize.x / _photoMapSize.y;

            if (bigAspect > mapAspect)
            {
                var relation = _photoMapSize.x / bigElementSize.x;
                bigElementRect.sizeDelta = new Vector2(_photoMapSize.x, relation * bigElementSize.y);
            }
            else
            {
                var relation = _photoMapSize.y / bigElementSize.y;
                bigElementRect.sizeDelta = new Vector2(relation * bigElementSize.x, _photoMapSize.y);
            }

            InnerPart(bigElementSize, viewSize);
        }

        public void _SetMap(Vector2 imageSize, Vector2 viewSize)
        {
            var pictureAspect = imageSize.x / imageSize.y;
            var mapAspect = _photoMapSize.x / _photoMapSize.y;

            if (pictureAspect > mapAspect)
            {
                var relation = _photoMapSize.x / imageSize.x;
                _picture.sizeDelta = new Vector2(_photoMapSize.x, relation * imageSize.y);
            }
            else
            {
                var relation = _photoMapSize.y / imageSize.y;
                _picture.sizeDelta = new Vector2(relation * imageSize.x, _photoMapSize.y);
            }

            InnerPart(imageSize, viewSize);
        }

        private void InnerPart(Vector2 imageSize, Vector2 viewSize)
        {
            Vector2 relations = new Vector2(viewSize.x / imageSize.x, viewSize.y / imageSize.y);

            _map.sizeDelta = new Vector2(_photoMapSize.x * relations.x, _photoMapSize.y * relations.y);

            //  Debug.Log($"mapsizedelta ({_map.sizeDelta.x},{_map.sizeDelta.y}) ; relations {relations}");
        }
    }
}