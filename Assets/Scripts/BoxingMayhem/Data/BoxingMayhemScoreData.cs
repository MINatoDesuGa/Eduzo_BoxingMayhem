namespace Eduzo.Games.BoxingMayhem.Data {
    public class BoxingMayhemScoreData {
        private const int DEFAULT_MAX_SCORE = 100;

        private float _score;
        private int _maxScore;
        private int _currentTotalCorrectAnswerCount;
        private int _totalQuestions;
        public BoxingMayhemScoreData(int totalQuestions) {
            _score = _currentTotalCorrectAnswerCount = 0;
            _totalQuestions = totalQuestions;
            _maxScore = DEFAULT_MAX_SCORE;
        }

        public void UpdateCorrectAnswerCount() {
            ++_currentTotalCorrectAnswerCount;
        }
        public void CalculateScore(out float totalScore) {
            _score = totalScore = ((float)_currentTotalCorrectAnswerCount / (float)_totalQuestions) * (float) _maxScore;
        }
        public void IsFullScoreObtained(out bool isFullScore) {
            isFullScore = (int)_score == _maxScore;
        }
        public void Reset() {
            _score = _currentTotalCorrectAnswerCount = 0;
        }
    }
}