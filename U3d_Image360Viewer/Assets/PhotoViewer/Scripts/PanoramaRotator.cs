using UnityEngine;

namespace PhotoViewer.Scripts
{
    public class PanoramaRotator : MonoBehaviour
    {
        public void OnRotate(float deltaValue)
        {
            Vector3 rotation =transform.rotation.eulerAngles + Vector3.up*deltaValue;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}