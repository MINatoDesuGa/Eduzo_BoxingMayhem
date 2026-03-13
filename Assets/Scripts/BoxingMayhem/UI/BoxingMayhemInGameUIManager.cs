using DG.Tweening;
using Eduzo.Games.Utility;
using TMPro;
using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemInGameUIManager : MonoBehaviour {
        private const float QUESTION_BG_ANIMATION_DURATION = 0.5f;

        [SerializeField] private CanvasGroup _inGameCanvasGroup;
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private RectTransform _questionBGRectTransform;
        [SerializeField] private UnityEngine.UI.Image _answerFeedbackImage;
        [SerializeField] private Sprite _correctFeedbackSprite;
        [SerializeField] private Sprite _incorrectFeedbackSprite;

        private Tween _answerFeedbackTween;
        private Sequence _questionUpdateAnimationSequence;
        private Vector2 _activeQuestionBGSizeDelta;
        private Vector2 _inactiveQuestionBGSizeDelta;
        #region Mono
        private void OnEnable() {
            BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected += OnPracticeModeSelected;
            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
            BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit += OnQuestionsInit;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemQuestionUpdate += UpdateQuestionText;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemAnswerValidated += OnAnswerValidated;
        }
        private void Start() {
            _activeQuestionBGSizeDelta = _questionBGRectTransform.sizeDelta;
            _inactiveQuestionBGSizeDelta = new Vector2(_questionBGRectTransform.sizeDelta.x, 0f);
            _questionBGRectTransform.sizeDelta = _inactiveQuestionBGSizeDelta;
            _questionText.DOFade(0f, 0.1f);
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
            _questionUpdateAnimationSequence = DOTween.Sequence();
            _questionUpdateAnimationSequence.Append(DoQuestionUpdateAnimation(true))
                                            .AppendCallback(() => {
                                                _questionText.DOFade(1f, QUESTION_BG_ANIMATION_DURATION).SetEase(Ease.OutQuad);
                                                _questionText.SetText(question);
                                            }).Play();
        }
        private void OnQuitToHome() {
            _inGameCanvasGroup.DisableCanvasGroup();
        }
        private void OnAnswerValidated(bool isCorrectAns) {
            _answerFeedbackImage.sprite = (isCorrectAns ? _correctFeedbackSprite : _incorrectFeedbackSprite);
            _answerFeedbackTween?.Kill();

            _answerFeedbackTween = _answerFeedbackImage.transform.DoScaleAnimation();
            _questionText.SetText(string.Empty);
            DoQuestionUpdateAnimation(false);
            _questionText.DOFade(0f, 0.1f);
        }
        #endregion


        #region Animations
        private Tween DoQuestionUpdateAnimation(bool isActivating) {
            Vector2 targetSizeDelta = _activeQuestionBGSizeDelta;
            if (!isActivating) {
                targetSizeDelta = _inactiveQuestionBGSizeDelta;
            }
            return _questionBGRectTransform.DOSizeDelta(targetSizeDelta, QUESTION_BG_ANIMATION_DURATION).SetEase(Ease.OutQuad);
        }

        #endregion
    }
}