using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Eduzo.Games.Utility;

namespace Eduzo.Games.BoxingMayhem.UI {
    [RequireComponent(typeof(Button))]
    public class BoxingMayhemPunchBagUIManager : MonoBehaviour {
        public static event System.Action<bool> OnBoxingMayhemPunchBagClick;
        private const float PUNCH_BAG_Z_ROTATION_MAX_OFFSET = 10f;
        private const float PUNCH_BAG_ANIMATION_DURATION = 0.25f;
        private const float PUNCH_IMPACT_ANIMATION_FADE_DURATION = 0.5f;

        [SerializeField] private bool _isTrueAnswerBag;
        [SerializeField] private RectTransform _parentTransform;
        [SerializeField] private Outline _bagOutline;
        [SerializeField] private Image _punchImpactImage;

        private Button _punchBagButton;
        private Tween _punchBagAnimationTween, _punchImpactAnimationTween;
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
            //_punchImpactAnimationObject.SetActive(true);
            DoPunchImpactAnimation();
            Invoke(nameof(DelayDisable), PUNCH_BAG_ANIMATION_DURATION);
            if (_isTrueAnswerBag) {
                DoPunchBagAnimation(-PUNCH_BAG_Z_ROTATION_MAX_OFFSET);
            } else {
                DoPunchBagAnimation(PUNCH_BAG_Z_ROTATION_MAX_OFFSET);
            }

        }
        #endregion
        private void DelayDisable() {
            _bagOutline.enabled = false;
            //_punchImpactAnimationObject.SetActive(false);
        }
        private void DoPunchBagAnimation(float maxZOffset) {
            _punchBagAnimationTween?.Kill();
            _punchBagAnimationTween = _parentTransform.DOBlendablePunchRotation(
                    _parentTransform.localRotation.eulerAngles + (Vector3.forward * maxZOffset),
                    PUNCH_BAG_ANIMATION_DURATION
                );
        }
        private void DoPunchImpactAnimation() {
            _punchImpactAnimationTween?.Kill();
            _punchImpactImage.transform.localScale = Vector3.zero;
            _punchImpactImage.DOFade(1f, 0.1f);
            _punchImpactAnimationTween = _punchImpactImage.transform.DOScale(Vector3.one, PUNCH_BAG_ANIMATION_DURATION).OnComplete(()=> {
                _punchImpactImage.DOFade(0f, PUNCH_IMPACT_ANIMATION_FADE_DURATION);
            });
        }
    }
}