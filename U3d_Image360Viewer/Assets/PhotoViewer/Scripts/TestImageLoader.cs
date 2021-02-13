using System;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class TestImageLoader : MonoBehaviour
    {
        [SerializeField] private PhotoViewer _photoViewer;
        [SerializeField] private TestImageLibrary _imageLibrary;

        private void Start()
        {
            _photoViewer.AddImageData(_imageLibrary.ImageDatas);
            
            _photoViewer.Show();
        }
    }
}