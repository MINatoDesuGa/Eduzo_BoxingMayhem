using UnityEngine;
using UnityEngine.UI;

namespace Eduzo.Games.BoxingMayhem.UI {
    [RequireComponent(typeof(Button))]
    public class BoxingMayhemPunchBagUIManager : MonoBehaviour {
        public static event System.Action<bool> OnBoxingMayhemPunchBagClick;

        [SerializeField] private bool _isTrueAnswerBag;

        private Button _punchBagButton;
        #region Mono
        private void Awake() {
            _punchBagButton = GetComponent<Button>();
        }
        private void OnEnable() {
            _punchBagButton.onClick?.AddListener(OnPunchBagClick);
        }
        private void OnDisable() {
            _punchBagButton.onClick?.RemoveListener(OnPunchBagClick);
        }
        #endregion

        #region Event Listeners
        private void OnPunchBagClick() {
            //print($"{_isTrueAnswerBag} bag click");
            OnBoxingMayhemPunchBagClick?.Invoke(_isTrueAnswerBag);
        }
        #endregion
    }
}