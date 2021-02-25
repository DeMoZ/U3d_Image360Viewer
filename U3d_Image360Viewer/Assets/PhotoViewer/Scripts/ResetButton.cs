using UnityEngine;
using UnityEngine.UI;

namespace PhotoViewer.Scripts
{
    [RequireComponent(typeof(Button))]
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] private Button _button = null;

        public void Show(bool show)
        {
            if (show == _button.interactable) return;

            _button.interactable = show;
        }
    }
}