namespace Eduzo.Games.BoxingMayhem.Data {
    public class BoxingMayhemSaveDataHandler : Utility.SaveManager<SaveData> {
        private SaveData _saveData;
        private QuestionResponseData _questionResponseData;
        public BoxingMayhemSaveDataHandler() {
            _saveData = new SaveData();
            _questionResponseData = new QuestionResponseData();
        }
        public void SaveResult() {
            SaveGameAsync(Utility.GameName.BoxingMayhem, _saveData);
        }
        #region Updaters
        public void UpdateQuestionResponseData(string question, bool userResponse, bool correctAnswer) {
            _questionResponseData.Update(question, userResponse, correctAnswer);
        }
        public void UpdateSaveData(SaveType saveType, object data = null) {
            switch (saveType) {
                case SaveType.Score:
                    _saveData.TotalScore = (float)data;
                    break;
                case SaveType.ActiveTime:
                    _saveData.ActiveTime = (int)data;
                    break;
                case SaveType.ResponseData:
                    _saveData.TotalResponses++;
                    if (_questionResponseData.UserResponse == _questionResponseData.CorrectAnswer) {
                        _saveData.TotalCorrectResponses++;
                    } else {
                        _saveData.TotalIncorrectResponses++;
                    }
                    _saveData.QuestionResultCollection.Add(_questionResponseData);
                    break;
            }
        }
        public void ResetData() {
            _saveData.Reset();
        }
        #endregion
    }
    public struct QuestionResponseData {
        public string Question;
        public bool CorrectAnswer;
        public bool UserResponse;
        public QuestionResponseData Update(string question, bool userResponse, bool correctAnswer) {
            Question = question;
            UserResponse = userResponse;
            CorrectAnswer = correctAnswer;
            return this;
        }
    }
    public class SaveData {
        public float TotalScore;
        public int ActiveTime;
        public int TotalResponses;
        public int TotalCorrectResponses;
        public int TotalIncorrectResponses;
        public System.Collections.Generic.List<QuestionResponseData> QuestionResultCollection;
        public SaveData() {
            TotalScore = 0f;
            TotalResponses = 0;
            TotalCorrectResponses = 0;
            TotalIncorrectResponses = 0;
            QuestionResultCollection = new();
        }
        public void Reset() {
            TotalScore = 0f;
            TotalResponses = 0;
            TotalCorrectResponses = 0;
            TotalIncorrectResponses = 0;
            QuestionResultCollection.Clear();
        }
    }
    public enum SaveType {
        Score, ResponseData, ActiveTime
    }
}