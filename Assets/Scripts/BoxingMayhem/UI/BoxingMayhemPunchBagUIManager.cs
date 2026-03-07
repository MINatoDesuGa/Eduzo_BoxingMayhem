using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Eduzo.Games.BoxingMayhem.UI {
    [RequireComponent(typeof(Button))]
    public class BoxingMayhemPunchBagUIManager : MonoBehaviour {
        public static event System.Action<bool> OnBoxingMayhemPunchBagClick;
        private const float PUNCH_BAG_Z_ROTATION_MAX_OFFSET = 10f;
        private const float PUNCH_BAG_ANIMATION_DURATION = 0.25f;

        [SerializeField] private bool _isTrueAnswerBag;
        [SerializeField] private RectTransform _parentTransform;
        [SerializeField] private Outline _bagOutline;

        private Button _punchBagButton;
        private Tween _punchBagAnimationTween;
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
            _bagOutline.enabled = true;
            Invoke(nameof(DelayDisableOutline), 0.1f);
            if (_isTrueAnswerBag) {
                DoPunchBagAnimation(-PUNCH_BAG_Z_ROTATION_MAX_OFFSET);
            } else {
                DoPunchBagAnimation(PUNCH_BAG_Z_ROTATION_MAX_OFFSET);
            }

        }
        #endregion
        private void DelayDisableOutline() {
            _bagOutline.enabled = false;
        }
        private void DoPunchBagAnimation(float maxZOffset) {
            _punchBagAnimationTween?.Kill();
            _punchBagAnimationTween = _parentTransform.DOBlendablePunchRotation(
                    _parentTransform.localRotation.eulerAngles + (Vector3.forward * maxZOffset),
                    PUNCH_BAG_ANIMATION_DURATION
                );
        }
    }
}