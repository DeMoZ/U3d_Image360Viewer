using UnityEngine;

namespace PhotoViewer.Scripts.Panorama
{
    public class PanoramaRotator : MonoBehaviour
    {
        public void OnRotate(Vector2 deltaValue)
        {
            var euler = transform.rotation.eulerAngles;
            euler = new Vector3(euler.x + -deltaValue.y, euler.y - deltaValue.x, euler.z);

            if (euler.x > 180)
                euler.x -= 360;

            euler.x = Mathf.Clamp(euler.x, -60, 40);

            transform.rotation = Quaternion.Euler(euler);
        }
    }
}