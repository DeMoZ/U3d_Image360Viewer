using System;
using System.Collections.Generic;
using PhotoViewer.Scripts.Gallery;
using PhotoViewer.Scripts.Panorama;
using PhotoViewer.Scripts.Photo;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public class PhotoViewer : MonoBehaviour
    {
        [SerializeField] private GalleryView _galleryView = default;
        [SerializeField] private PanoramaView _panoramaView = default;
        [SerializeField] private PhotoView _photoView = default;
        [SerializeField] private GameObject _bottomPanel = default;
        [SerializeField] private GameObject _btnRotLeft = default;
        [SerializeField] private GameObject _btnRotRight = default;
        [SerializeField] private GameObject _btnReturn = default;
        [SerializeField] private ResetButton _btnReset = default;
        [SerializeField] private Sprite _imageDefault = default;
        [SerializeField] private Text _imageName = default;
        [SerializeField] private Text _imageDate = default;
        [SerializeField] private Slider _zoomSlider = default;

        private List<ImageData> _images = new List<ImageData>();
        private int _currentPhoto { get; set; }

        private IPhotoView _currentView;
        private ImageData _currentImageData;

        private event Action ShowNewImage;
        public event Action CloseImageViewer;

        private void Start()
        {
            _zoomSlider.onValueChanged.AddListener(Zoom);
            _btnReset.Show(false);
            _btnReturn.SetActive(false);
            
            _photoView.SubscribeMeOnChange(() =>
            {
                _btnReset.Show(true);
                _photoView.ShowMap(true);
            });

            _panoramaView.SubscribeMeOnChange(() =>
            {
                _btnReset.Show(true);
                _panoramaView.ShowMap(true);
            });
        }

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
            _photoView.ShowImage(_imageDefault);
            ResetZoom();
            _btnReset.Show(false);
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
            _bottomPanel.gameObject.SetActive(false);
            _galleryView.gameObject.SetActive(true);

            _imageName.text = Globals.GalleryViewHeader;
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

        public void SetZoomSlider(float value)
        {
            if (_currentView != null)
            {
                if (_currentView.GetType() == typeof(PhotoView))
                    _zoomSlider.value -= value;
                else
                    _zoomSlider.value += value;
            }
        }

        public void Return() =>
            EnableGalleryViewState();

        public void SubscribeMeOnNewImage(Action callback) =>
            ShowNewImage += callback;

        public void UnSubscribeMeOnNewImage(Action callback) =>
            ShowNewImage -= callback;

        public void ResetCurrentImage() =>
            ShowImage(_currentImageData);

        private void Zoom(float value)
        {
            _btnReset.Show(true);
            _currentView?.ShowMap(true);
            _currentView?.Zoom(_zoomSlider.value);
        }

        private void ShowImage(int number)
        {
            _galleryView.gameObject.SetActive(false);
            _bottomPanel.gameObject.SetActive(true);
            _btnReturn.SetActive(true);

            _currentPhoto = number;
            ShowImage(_images[number]);
        }

        private void ShowImage(ImageData imageData)
        {
            _currentImageData = imageData;

            ShowNewImage?.Invoke();

            _imageName.text = imageData.Name;
            _imageDate.text = imageData.Date;
            // var dt = DateTime.Parse(list[n].Key);
            // _imageName.text = dt.ToString("dd MMMM yyyy", new CultureInfo("ru-RU")).ToLower();

            if (IsPhoto(imageData.Sprite))
            {
                _photoView.GetComponent<ViewerInput>().Clear();
                ResetZoom();
                _photoView.gameObject.SetActive(true);
                _panoramaView.gameObject.SetActive(false);
                ActivateRotateButtons(true);
                _currentView = _photoView;
            }
            else
            {
                _panoramaView.GetComponent<ViewerInput>().Clear();
                ResetZoom(0.5f);
                _photoView.gameObject.SetActive(false);
                _panoramaView.gameObject.SetActive(true);
                ActivateRotateButtons(false);
                _currentView = _panoramaView;
            }

            _currentView.ShowImage(imageData.Sprite);
            _currentView.ShowMap(false);
            _btnReset.Show(false);
        }

        private void ResetZoom(float value = 0)
        {
            _zoomSlider.onValueChanged.RemoveListener(Zoom);
            _zoomSlider.value = value;
            _zoomSlider.onValueChanged.AddListener(Zoom);
        }

        private bool IsPhoto(Sprite sprite) =>
            sprite.texture.width / sprite.texture.height < 1.6f;

        private void OnDestroy() =>
            _zoomSlider.onValueChanged.RemoveAllListeners();

        private void ActivateRotateButtons(bool activate)
        {
            _btnRotLeft.SetActive(activate);
            _btnRotRight.SetActive(activate);
        }
    }
}