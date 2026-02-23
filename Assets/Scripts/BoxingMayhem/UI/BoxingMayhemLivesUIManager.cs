using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemLivesUIManager : MonoBehaviour {
        public static event System.Action<Controller.GameOverType> OnBoxingMayhemLivesUp;

        [SerializeField] private Utility.CanvasGroupToggle _livesCanvasGroupToggle;
        [SerializeField] private System.Collections.Generic.List<Utility.SpriteToggle> _livesImages;

        private Data.BoxingMayhemLivesData _livesData = new();
        #region Mono
        private void OnEnable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemReplay += OnGameReplay;
        }
        private void OnDisable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected -= OnTestModeSelected;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome -= OnQuitToHome;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemReplay -= OnGameReplay;
        }
        private void Reset() {
            _livesData.Reset();
            ResetLivesImages();
        }
        #endregion

        #region Event Listeners
        private void OnTestModeSelected() {
            ActivateInGameEventsListening(true); //TODO: unsubscribe on exit to main menu
            _livesData.Reset();

            if(_livesCanvasGroupToggle.IsVisible()) return;
            _livesCanvasGroupToggle.Toggle();
        }
        private void OnAnswerValidated(bool isAnswerCorrect) {
            if (!isAnswerCorrect) {
                UpdateLivesImage(_livesData.GetCurrentLivesIndex());
                _livesData.DeductCurrentLives();
                _livesData.CheckWhetherLivesLeft(out bool isLivesLeft);

                if(!isLivesLeft) {
                    OnBoxingMayhemLivesUp?.Invoke(Controller.GameOverType.LivesUp);
                }
            }
        }
        private void OnGameReplay() {
            Reset();
        }
        private void OnQuitToHome() {
            Reset();
            if(Controller.BoxingMayhemGameFlowHandler.CurrentGameMode is not Controller.GameMode.Test) return;

            ActivateInGameEventsListening(false);
        }
        #endregion
        private void ActivateInGameEventsListening(bool active) {
            if (!active) {
                Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemAnswerValidated -= OnAnswerValidated;
            } else {
                Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemAnswerValidated += OnAnswerValidated;
            }
        }
        private void UpdateLivesImage(int index) {
            _livesImages[index].Toggle();
        }
        private void ResetLivesImages() {
            for (int i = 0; i < _livesImages.Count; i++) {
                _livesImages[i].Reset();
            }
        }
    }
}