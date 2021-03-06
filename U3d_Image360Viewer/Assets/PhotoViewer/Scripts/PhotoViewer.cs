using System;
using System.Collections.Generic;
using PhotoViewer.Scripts.Photo;
using PhotoViewer.Scripts.Gallery;
using PhotoViewer.Scripts.Panorama;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class PhotoViewer : MonoBehaviour
    {
        [SerializeField] private GalleryView _galleryView = default;
        [SerializeField] private PanoramaView _panoramaView = default;

        [SerializeField] private PhotoView _photoView = default;
        [SerializeField] private GameObject _btnReturn = default;

        private List<ImageData> _images = new List<ImageData>();
        private int _currentPhoto { get; set; }

        private IView _currentView;
        private ImageData _currentImageData;

        private event Action ShowNewImage;
        public event Action CloseImageViewer;

        private void Start() =>
            _btnReturn.SetActive(false);

        public void CloseViewer()
        {
            Clear();
            CloseImageViewer?.Invoke();

            gameObject.SetActive(false);
        }

        public void AddImageData(ImageData data) =>
            _images.Add(data);

        public void AddImageData(List<ImageData> data) =>
            _images.AddRange(data);

        public void Clear()
        {
            _images.Clear();

            _currentImageData = new ImageData();
            _galleryView.Clear();
            _panoramaView.Clear();
            _photoView.Clear();
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (_galleryView)
            {
                EnableGalleryViewState();
                _galleryView.Init(_images);
                _galleryView.OnImageSelect += ShowImage;
            }
            else
            {
                if (_images != null && _images.Count > 0)
                {
                    _currentPhoto = 0;
                    ShowImage(_images[0]);
                }
                else
                    Clear();
            }
        }

        private void EnableGalleryViewState()
        {
            _panoramaView.gameObject.SetActive(false);
            _photoView.gameObject.SetActive(false);
            _btnReturn.SetActive(false);
            _galleryView.gameObject.SetActive(true);
        }

        public void NextImage()
        {
            _currentPhoto = (++_currentPhoto > _images.Count - 1) ? 0 : _currentPhoto;
            ShowImage(_images[_currentPhoto]);
        }

        public void PrevImage()
        {
            _currentPhoto = (_currentPhoto <= 0) ? _images.Count - 1 : _currentPhoto - 1;
            ShowImage(_images[_currentPhoto]);
        }


        public void Return() =>
            EnableGalleryViewState();

        public void SubscribeMeOnNewImage(Action callback) =>
            ShowNewImage += callback;

        public void UnSubscribeMeOnNewImage(Action callback) =>
            ShowNewImage -= callback;

        public void ResetCurrentImage() =>
            ShowImage(_currentImageData);

        private void ShowImage(int number)
        {
            _galleryView.gameObject.SetActive(false);
            _btnReturn.SetActive(true);

            _currentPhoto = number;
            ShowImage(_images[number]);
        }

        private void ShowImage(ImageData imageData)
        {
            _currentImageData = imageData;

            ShowNewImage?.Invoke();

            if (IsPhoto(imageData.Sprite))
            {
                _photoView.gameObject.SetActive(true);
                _panoramaView.gameObject.SetActive(false);
                _currentView = _photoView;
                _photoView.Show(imageData);
            }
            else
            {
                _photoView.gameObject.SetActive(false);
                _panoramaView.gameObject.SetActive(true);
                _currentView = _panoramaView;
                _panoramaView.Show(imageData);
            }
        }


        private bool IsPhoto(Sprite sprite) =>
            sprite.texture.width / sprite.texture.height < 1.6f;
    }

    public interface IView
    {
    }
}