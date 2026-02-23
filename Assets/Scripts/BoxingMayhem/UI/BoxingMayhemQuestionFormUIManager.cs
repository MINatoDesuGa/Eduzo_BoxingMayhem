using Eduzo.Games.BoxingMayhem.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemQuestionFormUIManager : MonoBehaviour {
        public static event System.Action<System.Collections.Generic.List<Data.QuestionData>> OnBoxingMayhemQuestionInit;

        [SerializeField] private Utility.CanvasGroupToggle _questionFormCanvasGroupToggle;
        [SerializeField] private TMP_InputField _questionField;
        [SerializeField] private TMP_Dropdown _answerDropdown;
        [SerializeField] private Button _confirmQuestionButton;
        [SerializeField] private Button _playButton;


        private Data.QuestionData _currentQuestionData = new();
        private Data.BoxingMayhemQuestionFormData _questionFormData = new();
        #region Mono
        private void OnEnable() {
            _questionField.onValueChanged?.AddListener(OnQuestionFieldValueChanged);
            _answerDropdown.onValueChanged?.AddListener(OnAnswerDropdownValueChanged);
            _confirmQuestionButton.onClick?.AddListener(OnConfirmQuestionButtonClicked);
            _playButton.onClick?.AddListener(OnFormCloseButtonClicked);
        }
        private void Start() {
            _currentQuestionData.Answer = true;
        }
        private void OnDisable() {
            _questionField.onValueChanged?.RemoveListener(OnQuestionFieldValueChanged);
            _answerDropdown.onValueChanged?.RemoveListener(OnAnswerDropdownValueChanged);
            _confirmQuestionButton.onClick?.RemoveListener(OnConfirmQuestionButtonClicked);
            _playButton.onClick?.RemoveListener(OnFormCloseButtonClicked);
        }
        private void Reset() {
            _questionField.text = string.Empty;
        }
        #endregion

        #region Event Listeners
        private void OnQuestionFieldValueChanged(string questionInput) {
            _currentQuestionData.Question = questionInput;
        }
        private void OnAnswerDropdownValueChanged(int value) {
            _currentQuestionData.Answer = (value != 1);
        }
        private void OnConfirmQuestionButtonClicked() {
            BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.ButtonClick);
            ValidateUserInputs(out bool isUserInputValid);

            if (!isUserInputValid) {
                Debug.LogError("Invalid user input");
                return;
            }
            _questionFormData.AddQuestionData(_currentQuestionData);
            Reset();
        }
        private void OnFormCloseButtonClicked() {
            BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.ButtonClick);
            _questionFormCanvasGroupToggle.Toggle();
            OnBoxingMayhemQuestionInit?.Invoke(_questionFormData.GetQuestionCollection());
        }
        #endregion
        private void ValidateUserInputs(out bool isUserInputValid) {
            if (_questionField.text == string.Empty) {
                isUserInputValid = false;
                return;
            }
            isUserInputValid = true;
        }
    }
}