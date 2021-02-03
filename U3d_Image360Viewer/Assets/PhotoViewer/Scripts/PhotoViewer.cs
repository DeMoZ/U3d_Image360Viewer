using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    public class PhotoViewer : MonoBehaviour
    {
        [SerializeField] private Image _photoImage;
        [SerializeField] private RawImage _panoramaRawImage;
        [SerializeField] private PanoramaView _panoramaView;
        [SerializeField] private GameObject _btnNext;
        [SerializeField] private GameObject _btnPrev;
        [SerializeField] private Sprite _imageDefault;
        [SerializeField] private Text  _imageName;
        public string[] photoDates { get; set; }

        private int tempCount = 0;


        public event Action MainBack;

        public void CloseViewer() =>
            MainBack?.Invoke();

        private Dictionary<string, Sprite> photos = new Dictionary<string, Sprite>();

        private int currentPhoto { get; set; }

        private Image getPhotoImage =>
            _photoImage.GetComponent<Image>();

        public void SetPhotoDates(string[] dates)
        {
            photoDates = dates;
            for (int i = 0; i < photoDates.Length; ++i)
            {
                photoDates[i] = photoDates[i].Trim();
                Debug.Log(photoDates[i]);
            }
        }

        public void SwitchOnOff(float to, float time)
        {
            if (to != 0) gameObject.SetActive(true);

            // LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), to, time).setOnComplete(() =>
            // {
            //     if (to == 0) gameObject.SetActive(false);
            // });
        }

        public void UpdatePhotos()
        {
            Debug.Log("photos update");
            if (photos.Count == 0)
            {
                getPhotoImage.sprite = _imageDefault;
                _imageName.text = "";
                RescalePhoto();
            }
            else
                ShowPhoto(0);

            _btnNext.SetActive(photos.Count > 1);
            _btnPrev.SetActive(photos.Count > 1);
        }

        public void NextPhoto()
        {
            if (photos.Count > 0)
                ShowPhoto((currentPhoto + 1) % photos.Count);
        }

        public void PrevPhoto()
        {
            if (photos.Count > 0)
                ShowPhoto(currentPhoto == 0 ? photos.Count - 1 : (currentPhoto - 1) % photos.Count);
        }

        public void Clear() =>
            photos.Clear();

        public void OnTextureLoaded(Texture2D texture, string fileName)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(.5f, .5f));
            photos[fileName.Split('/')[0]] = sprite;
            UpdatePhotos();
        }


        public void LoadImage(string fileName)
        {
            TextureLoader textureLoader = new TextureLoader();
            textureLoader.LoadData(fileName, OnTextureLoaded, OnTextureLoadError);
        }

        private void ShowPhoto(int n)
        {
            currentPhoto = n;
            List<KeyValuePair<string, Sprite>> list = new List<KeyValuePair<string, Sprite>>(photos);

            bool isPhoto = tempCount++ % 2 == 0;

            Sprite sprite = list[n].Value;

            if (IsPhoto(sprite))
            {
                _photoImage.gameObject.SetActive(true);
                _panoramaView.gameObject.SetActive(false);
                getPhotoImage.sprite = sprite;
                RescalePhoto();
            }
            else // Panorama
            {
                _photoImage.gameObject.SetActive(false);
                _panoramaView.gameObject.SetActive(true);
                _panoramaView.ApplyPicture(sprite);

                _panoramaView.LeanIcon();

                RescalePanorama();
            }

            var dt = DateTime.Parse(list[n].Key);
            _imageName.text = dt.ToString("dd MMMM yyyy", new CultureInfo("ru-RU")).ToLower();
        }

        private void RescalePhoto()
        {
            var s = getPhotoImage.sprite;
            float scale = Mathf.Min(1024.0f / s.texture.width, 1024.0f / s.texture.height);

            float aspect = 1.0f * s.texture.width / s.texture.height;

            GetComponent<RectTransform>().sizeDelta = new Vector2(scale * s.texture.width, scale * s.texture.height);

            Debug.Log(aspect + " " + s.texture.width + " " + s.texture.height);
        }

        private void RescalePanorama()
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(1024, 768);
        }

        private void OnTextureLoadError()
        {
        }

        private bool IsPhoto(Sprite sprite)
        {
            if (sprite.texture.width / sprite.texture.height > 1.6f)
                return false;

            return true;
        }
    }
}