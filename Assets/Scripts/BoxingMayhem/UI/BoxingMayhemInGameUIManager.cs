using DG.Tweening;
using Eduzo.Games.Utility;
using TMPro;
using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemInGameUIManager : MonoBehaviour {
        [SerializeField] private CanvasGroup _inGameCanvasGroup;
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private UnityEngine.UI.Image _answerFeedbackImage;
        [SerializeField] private Sprite _correctFeedbackSprite;
        [SerializeField] private Sprite _incorrectFeedbackSprite;

        private Tween _answerFeedbackTween;
        #region Mono
        private void OnEnable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected += OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit += OnQuestionsInit;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemQuestionUpdate += UpdateQuestionText;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemAnswerValidated += OnAnswerValidated;
        }
        private void OnDisable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected -= OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected -= OnTestModeSelected;
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit -= OnQuestionsInit;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemQuestionUpdate -= UpdateQuestionText;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome -= OnQuitToHome;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemAnswerValidated -= OnAnswerValidated;
        }
        #endregion

        #region Event Listeners
        private void OnPracticeModeSelected() {
            _inGameCanvasGroup.EnableCanvasGroup();
        }
        private void OnTestModeSelected() {
            _inGameCanvasGroup.EnableCanvasGroup();
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
            _inGameCanvasGroup.DisableCanvasGroup();
        }
        private void OnAnswerValidated(bool isCorrectAns) {
            _answerFeedbackImage.sprite = (isCorrectAns ? _correctFeedbackSprite : _incorrectFeedbackSprite);
            _answerFeedbackTween?.Kill();

            _answerFeedbackTween = _answerFeedbackImage.transform.DoScaleAnimation();
        }
        #endregion
    }
}