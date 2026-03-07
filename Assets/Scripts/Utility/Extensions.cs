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

        #region ANIMATION
        private const float PUNCH_SCALE_DURATION = 0.2f;
        private const float PUNCH_SCALE_FACTOR = 0.01f;

        public static void DoPunchScaleAnimation(this Transform transform) {
            transform.DOPunchScale(transform.localScale + (Vector3.one * PUNCH_SCALE_FACTOR), PUNCH_SCALE_DURATION);
        }
        #endregion
    }
}