using UnityEngine;

namespace Eduzo.Games.Utility {
    [RequireComponent (typeof(CanvasGroup))]
    public class CanvasGroupToggle : MonoBehaviour {
        private CanvasGroup _canvasGroup;
        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup> ();
        }
        public void Toggle () { 
            _canvasGroup.alpha = _canvasGroup.alpha is 1 ? 0 : 1;
            _canvasGroup.interactable = _canvasGroup.interactable is true ? false : true;
            _canvasGroup.blocksRaycasts = _canvasGroup.blocksRaycasts is true ? false : true;
        }
        public bool IsVisible() {
            return _canvasGroup.alpha == 1;
        }
    }
}