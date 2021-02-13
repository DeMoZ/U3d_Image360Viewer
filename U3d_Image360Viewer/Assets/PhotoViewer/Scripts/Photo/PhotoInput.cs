using UnityEngine;

namespace PhotoViewer.Scripts.Photo
{
    [RequireComponent(typeof(IPhotoView))]
    public class PhotoInput : ViewerInput
    {
        private IPhotoView photoView;
        private void Start()
        {
            photoView = GetComponent<IPhotoView>();
        }

        protected override void OnUpdate()
        {
            photoView.ApplyInput(_deltaPosition);
        }
    }
}