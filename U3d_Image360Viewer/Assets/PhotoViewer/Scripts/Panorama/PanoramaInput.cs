using UnityEngine;

namespace PhotoViewer.Scripts.Panorama
{
    [RequireComponent(typeof(IPhotoView))]
    public class PanoramaInput : ViewerInput
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