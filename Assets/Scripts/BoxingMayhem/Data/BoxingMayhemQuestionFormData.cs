using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.Data {
    public class BoxingMayhemQuestionFormData {
        private System.Collections.Generic.List<QuestionData> _questionsCollection;
        private int _currentQuestionIndex;
        public BoxingMayhemQuestionFormData(System.Collections.Generic.List<QuestionData> questionDatas = null) {
            if (questionDatas != null) {
                _questionsCollection = questionDatas;
                return;
            }
            _questionsCollection = new();
            _currentQuestionIndex = 0;
        }

        public System.Collections.Generic.List<QuestionData> GetQuestionCollection() {
            return _questionsCollection;
        }
        public void AddQuestionData(QuestionData questionData) {
            _questionsCollection.Add(questionData);
        }
        public QuestionData GetCurrentQuestionData() {
            return _questionsCollection[_currentQuestionIndex];
        }
        public void UpdateCurrentQuestionIndex() {
            _currentQuestionIndex = Mathf.Clamp( _currentQuestionIndex + 1, 0, _questionsCollection.Count );
        }
        public int GetTotalQuestions() {
            return _questionsCollection.Count;
        }
        public void CheckWhetherQuestionCompleted(out bool isQuestionOver) {
            isQuestionOver = _currentQuestionIndex == _questionsCollection.Count;
        }
        public void Reset() {
            _currentQuestionIndex = 0;
        }
    }
    [System.Serializable]
    public struct QuestionData {
        public string Question;
        public bool Answer;
    }
}