using UnityEngine;

namespace PhotoViewer.Scripts.Photo
{
    [RequireComponent(typeof(AbstractView))]
    public class PhotoInput : AbstractInput
    {
        protected override void OnUpdate() =>
            _view.ApplyInput(_deltaPosition);
    }
}