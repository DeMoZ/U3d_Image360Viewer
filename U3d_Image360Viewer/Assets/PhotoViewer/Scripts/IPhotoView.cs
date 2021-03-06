using System;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public interface IPhotoView
    {
        void Zoom(float value);
        void ApplyInput(Vector2 deltaPosition);
        void ShowMap(bool show);
        void Show(ImageData imageData);
    }
}