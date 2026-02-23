using UnityEngine;
using UnityEngine.UI;

namespace Eduzo.Games.Utility {
    [RequireComponent(typeof(Image))]
    public class SpriteToggle : MonoBehaviour {
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _toggleSprite;

        private Image _image;
        #region Mono
        private void Awake() {
            _image = GetComponent<Image>();
        }
        private void Start() {
            Reset();
        }
        public void Reset() {
            _image.sprite = _defaultSprite;
        }
        #endregion
        public void Toggle() {
            _image.sprite = _defaultSprite ? _toggleSprite : _defaultSprite;
        }
        
    }
}