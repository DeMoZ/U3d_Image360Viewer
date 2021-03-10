using System.Collections.Generic;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class PhotoViewer : MonoBehaviour
    {
        [SerializeField] private Gallery.Gallery _gallery = default;
        [SerializeField] private Panorama.Panorama _panorama = default;
        [SerializeField] private Photo.Photo _photo = default;

        [SerializeField] private GameObject _btnReturn = default;
        [SerializeField] private GameObject _btnPrev = default;
        [SerializeField] private GameObject _btnNext = default;

        private List<ImageData> _images = new List<ImageData>();
        private int CurrentPhoto { get; set; }

        private ImageData _currentImageData;

        private void Start() =>
            _btnReturn.SetActive(false);

        public void AddImageData(ImageData data) =>
            _images.Add(data);

        public void AddImageData(List<ImageData> data) =>
            _images.AddRange(data);

        public void CloseViewer()
        {
            Clear();
            gameObject.SetActive(false);
        }

        public void Clear()
        {
            _images.Clear();

            _currentImageData = new ImageData();
            _gallery.Clear();
            _panorama.Clear();
            _photo.Clear();
        }

        public void Show()
        {
            if (_gallery)
            {
                EnableGalleryViewState();
                _gallery.Init(_images);
                _gallery.OnImageSelect += ShowImage;
            }
            else
            {
                if (_images != null && _images.Count > 0)
                {
                    CurrentPhoto = 0;
                    ShowImage(_images[0]);
                }
                else
                    Clear();
            }
        }

        public void NextImage()
        {
            CurrentPhoto = (++CurrentPhoto > _images.Count - 1) ? 0 : CurrentPhoto;
            ShowImage(_images[CurrentPhoto]);
        }

        public void PrevImage()
        {
            CurrentPhoto = (CurrentPhoto <= 0) ? _images.Count - 1 : CurrentPhoto - 1;
            ShowImage(_images[CurrentPhoto]);
        }

        public void Return() =>
            EnableGalleryViewState();

        public void ResetCurrentImage() =>
            ShowImage(_currentImageData);

        private void EnableGalleryViewState()
        {
            _panorama.gameObject.SetActive(false);
            _photo.gameObject.SetActive(false);
            SetActiveButtons(false);

            _gallery.gameObject.SetActive(true);
        }

        private void SetActiveButtons(bool active)
        {
            _btnReturn.SetActive(active);
            _btnPrev.SetActive(active);
            _btnNext.SetActive(active);
        }

        private void ShowImage(int number)
        {
            _gallery.gameObject.SetActive(false);
            SetActiveButtons(true);

            CurrentPhoto = number;
            ShowImage(_images[number]);
        }

        private void ShowImage(ImageData imageData)
        {
            _currentImageData = imageData;

            if (IsPhoto(imageData.Sprite))
            {
                _photo.gameObject.SetActive(true);
                _panorama.gameObject.SetActive(false);
                _photo.Show(imageData);
            }
            else
            {
                _photo.gameObject.SetActive(false);
                _panorama.gameObject.SetActive(true);
                _panorama.Show(imageData);
            }
        }

        private bool IsPhoto(Sprite sprite) =>
            sprite.texture.width / sprite.texture.height < 1.6f;
    }
}