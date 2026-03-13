using DG.Tweening;
using UnityEngine;

namespace Eduzo.Games.Utility {
    public static class Extensions {
        #region Canvas Group
        public static void EnableCanvasGroup(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        public static void DisableCanvasGroup(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        public static bool IsEnabled(this CanvasGroup canvasGroup) {
            return canvasGroup.alpha == 1;
        }
        #endregion

        #region Game Object
        public static void Enable(this GameObject gameObject) {
            gameObject.SetActive(true);
        }
        public static void Disable(this GameObject gameObject) {
            gameObject.SetActive(false);
        }
        #endregion

        #region ANIMATION
        private const float PUNCH_SCALE_DURATION = 0.2f;
        private const float PUNCH_SCALE_FACTOR = 0.01f;
        private const float SCALE_UP_DURATION = 0.5f;
        private const float SCALE_DOWN_DURATION = 0.5f;

        public static void DoPunchScaleAnimation(this Transform transform) {
            transform.DOPunchScale(transform.localScale + (Vector3.one * PUNCH_SCALE_FACTOR), PUNCH_SCALE_DURATION);
        }
        public static Tween DoScaleAnimation(this Transform transform) {
            return transform.DOScale(Vector3.one, SCALE_UP_DURATION).OnComplete(() => {
                transform.DOScale(Vector3.zero, SCALE_DOWN_DURATION);
            });
        }
        #endregion
    }
}