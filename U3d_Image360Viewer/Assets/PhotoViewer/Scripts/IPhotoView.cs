using System;
using UnityEngine;

namespace PhotoViewer.Scripts
{
    public interface IPhotoView
    {
        void Zoom(float value);
        void Clear();
        void ApplyInput(Vector2 deltaPosition);
        void SubscribeMeOnChange(Action callback);
        void UnSubscribeMeOnChange(Action callback);
    }
}