using Eduzo.Games.BoxingMayhem.Sound;
using Eduzo.Games.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemGameOverUIManager : MonoBehaviour {
        public static event System.Action OnBoxingMayhemReplay;
        public static event System.Action OnBoxingMayhemHome;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _homeButton;
        #region Mono
        private void OnEnable() {
            _replayButton.onClick.AddListener(OnReplayButtonClicked);
            _homeButton.onClick.AddListener(OnHomeButtonClicked);
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemGameOver += HandleGameOver;
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected += OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
        }
        private void OnDisable() {
            _replayButton.onClick.RemoveListener(OnReplayButtonClicked);
            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemGameOver -= HandleGameOver;
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected -= OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected -= OnTestModeSelected;
        }
        #endregion

        #region Event Listeners
        private void OnReplayButtonClicked() {
            BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.ButtonClick);

            _canvasGroup.DisableCanvasGroup();
            OnBoxingMayhemReplay?.Invoke();
        }

        private void OnHomeButtonClicked() {
            BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.ButtonClick);

            _canvasGroup.DisableCanvasGroup();
            OnBoxingMayhemHome?.Invoke();
        }
        private void OnPracticeModeSelected() {
            _scoreText.gameObject.SetActive(false);
        }
        private void OnTestModeSelected() {
            _scoreText.gameObject.SetActive(true);
        }
        private void HandleGameOver(Controller.GameOverType gameOverType, float score) {
            if (_canvasGroup.IsEnabled()) return;
            switch (gameOverType) {
                case Controller.GameOverType.TimeUp:
                    _titleText.text = "Time Up";
                    break;
                case Controller.GameOverType.QuestionCompleted:
                    _titleText.text = Controller.BoxingMayhemGameFlowHandler.CurrentGameMode is Controller.GameMode.Practice ?
                                      "Practice Completed" : "Test Completed";
                    break;
                case Controller.GameOverType.LivesUp:
                    _titleText.text = "Lives up";
                    break;
            }
            _scoreText.text = $"Final Score: {score:F2}/100";
            _canvasGroup.EnableCanvasGroup();
        }
        #endregion
    }
}