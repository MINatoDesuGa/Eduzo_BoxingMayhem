using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.Controller {
    public class BoxingMayhemCharacterController : MonoBehaviour {
        private const string PUNCH_TRIGGER = "Punch";

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        private Vector3 _oppositeVectorValue = new Vector3(-1 , 1, 1);
        #region Mono
        private void OnEnable() {
            UI.BoxingMayhemPunchBagUIManager.OnBoxingMayhemPunchBagClick += OnPunchBagClicked;
        }
        private void OnDisable() {
            UI.BoxingMayhemPunchBagUIManager.OnBoxingMayhemPunchBagClick -= OnPunchBagClicked;
        }
        #endregion

        #region Event Listeners
        private void OnPunchBagClicked(bool isTrueAnswerBag) {
            if(isTrueAnswerBag) {
                transform.localScale = _oppositeVectorValue;
            } else {
                transform.localScale = Vector3.one;
            }
            _animator.SetTrigger(PUNCH_TRIGGER); //TODO : differentiate between left and right punch bags
        }
        #endregion
    }
}