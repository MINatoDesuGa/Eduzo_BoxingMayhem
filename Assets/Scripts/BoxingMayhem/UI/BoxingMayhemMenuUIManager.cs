using Eduzo.Games.BoxingMayhem.Sound;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemMenuUIManager : MonoBehaviour {
        public static event Action OnBoxingMayhemPracticeModeSelected;
        public static event Action OnBoxingMayhemTestModeSelected;

        [SerializeField] private Utility.CanvasGroupToggle _menuCanvasGroupToggle;

        [SerializeField] private Button _practiceModeButton;
        [SerializeField] private Button _testModeButton;

        #region Mono
        private void OnEnable() {
            _practiceModeButton.onClick?.AddListener(OnPracticeModeSelected);
            _testModeButton.onClick?.AddListener(OnTestModeSelected);
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit += OnQuestionsUpdated;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
        }
        private void OnDisable() {
            _practiceModeButton.onClick?.RemoveListener(OnPracticeModeSelected);
            _testModeButton.onClick?.RemoveListener(OnTestModeSelected);
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit -= OnQuestionsUpdated;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome -= OnQuitToHome;
        }
        #endregion

        #region Event Listeners
        private void OnPracticeModeSelected() {
            BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.ButtonClick);

            Controller.BoxingMayhemGameFlowHandler.UpdateGameMode(Controller.GameMode.Practice);
            OnBoxingMayhemPracticeModeSelected?.Invoke();
            _menuCanvasGroupToggle.Toggle();
        }
        private void OnTestModeSelected() {
            BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.ButtonClick);

            Controller.BoxingMayhemGameFlowHandler.UpdateGameMode(Controller.GameMode.Test);
            OnBoxingMayhemTestModeSelected?.Invoke();
            _menuCanvasGroupToggle.Toggle();
        }
        private void OnQuestionsUpdated(System.Collections.Generic.List<Data.QuestionData> questionDatas) {
            _menuCanvasGroupToggle.Toggle();
        }
        private void OnQuitToHome() {
            _menuCanvasGroupToggle.Toggle();
        }
        #endregion
    }
}