using System;
using System.Collections.Generic;
using PhotoViewer.Scripts.Panorama;
using PhotoViewer.Scripts.Photo;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public class PhotoViewer : MonoBehaviour
    {
        [SerializeField] private PanoramaView _panoramaView;
        [SerializeField] private PhotoView _photoView;
        [SerializeField] private GameObject _btnNext;
        [SerializeField] private GameObject _btnPrev;
        [SerializeField] private Sprite _imageDefault;
        [SerializeField] private Text _imageName;
        [SerializeField] private Text _imageDate;
        [SerializeField] private Slider _zoomSlider;

        private List<ImageData> _images = new List<ImageData>();
        private int _currentPhoto { get; set; }

        private IPhotoView _currentView;

        public event Action CloseImageViewer;

        private void Start() =>
            _zoomSlider.onValueChanged.AddListener(Zoom);

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

            _btnPrev.SetActive(false);
            _btnNext.SetActive(false);
            _panoramaView.Clear();
            _photoView.Clear();
            _photoView.ShowImage(_imageDefault);
            ResetPhotoZoom();
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (_images != null && _images.Count > 0)
            {
                _currentPhoto = 0;
                ShowImage(_images[0]);
            }
            else
                Clear();
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

        private void Zoom(float value) =>
            _currentView?.Zoom(_zoomSlider.value);

        private void ShowImage(ImageData imageData)
        {
            _imageName.text = imageData.Name;
            _imageDate.text = imageData.Date;
            // var dt = DateTime.Parse(list[n].Key);
            // _imageName.text = dt.ToString("dd MMMM yyyy", new CultureInfo("ru-RU")).ToLower();

            if (IsPhoto(imageData.Sprite))
            {
                ResetPhotoZoom();
                _photoView.gameObject.SetActive(true);
                _panoramaView.gameObject.SetActive(false);

                _currentView = _photoView;

                _photoView.ShowImage(imageData.Sprite);
            }
            else
            {
                ResetPanoramaZoom(0.5f);
                _photoView.gameObject.SetActive(false);
                _panoramaView.gameObject.SetActive(true);

                _currentView = _panoramaView;

                _panoramaView.ShowImage(imageData.Sprite);
            }
        }

        private void ResetPhotoZoom()
        {
            _zoomSlider.onValueChanged.RemoveListener(Zoom);
            _zoomSlider.value = 0;
            _zoomSlider.onValueChanged.AddListener(Zoom);
        }

        private void ResetPanoramaZoom(float value) => 
            _zoomSlider.value = value;

        private bool IsPhoto(Sprite sprite) =>
            sprite.texture.width / sprite.texture.height < 1.6f;

        private void OnDestroy() =>
            _zoomSlider.onValueChanged.RemoveAllListeners();
    }
}