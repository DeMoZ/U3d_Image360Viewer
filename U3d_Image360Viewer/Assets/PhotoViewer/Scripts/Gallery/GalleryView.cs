using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts.Gallery
{
    public class GalleryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content = default;
        [SerializeField] private GalleryTile _galleryTilePrefab = default;

        private Coroutine _applySizeRoutine;

        public void Clear()
        {
            //   throw new NotImplementedException();

            if (_applySizeRoutine != null)
            {
                StopCoroutine(_applySizeRoutine);
                _applySizeRoutine = null;
            }
        }


        public void Init(List<ImageData> imageDatas)
        {
            foreach (var imageData in imageDatas)
            {
                var go = Instantiate(_galleryTilePrefab, Vector3.zero, Quaternion.identity, _content);
                var taleT = go.GetComponent<RectTransform>();

                go.Image.sprite = imageData.Sprite;
                var imageT = go.Image.GetComponent<RectTransform>();
                var relative = imageData.Sprite.rect.width / imageData.Sprite.rect.height;

                _applySizeRoutine = StartCoroutine(ApplySize(imageT, taleT, relative));

                if (go.Name)
                    go.Name.text = imageData.Name;

                if (go.Date)
                    go.Date.text = imageData.Date;
            }
        }

        private IEnumerator ApplySize(RectTransform imageT, RectTransform taleT, float relative)
        {
            yield return null;

            if (relative > 1)
                imageT.sizeDelta = new Vector2(taleT.rect.height * relative, taleT.rect.height);
            else
                imageT.sizeDelta = new Vector2(taleT.rect.height, taleT.rect.height * (2 - relative));

            _applySizeRoutine = null;
        }

        public void Show()
        {
            //     throw new NotImplementedException();
        }
    }
}