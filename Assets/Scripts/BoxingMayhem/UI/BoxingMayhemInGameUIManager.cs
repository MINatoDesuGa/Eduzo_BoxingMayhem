using TMPro;
using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemInGameUIManager : MonoBehaviour {
        [SerializeField] private Utility.CanvasGroupToggle _inGameCanvasGroupToggle;
        [SerializeField] private TMP_Text _questionText;

        #region Mono
        private void OnEnable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected += OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit += OnQuestionsInit;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemQuestionUpdate += UpdateQuestionText;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
        }
        private void OnDisable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected -= OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected -= OnTestModeSelected;
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit -= OnQuestionsInit;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemQuestionUpdate -= UpdateQuestionText;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome -= OnQuitToHome;
        }
        #endregion

        #region Event Listeners
        private void OnPracticeModeSelected() {
            _inGameCanvasGroupToggle.Toggle();
        }
        private void OnTestModeSelected() {
            _inGameCanvasGroupToggle.Toggle();
        }
        private void OnQuestionsInit(System.Collections.Generic.List<Data.QuestionData> questionDatas) {
#if UNITY_EDITOR
            foreach (var questionData in questionDatas) {
                print($"{questionData.Question} : {questionData.Answer}");
            }
#endif
        }
        private void UpdateQuestionText(string question) {
            _questionText.SetText(question);
        }
        private void OnQuitToHome() {
            _inGameCanvasGroupToggle.Toggle();
        }
        #endregion
        
    }
}