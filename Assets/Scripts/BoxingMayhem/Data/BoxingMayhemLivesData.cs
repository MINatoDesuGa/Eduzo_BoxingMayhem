namespace Eduzo.Games.BoxingMayhem.Data {
    public class BoxingMayhemLivesData {
        private const int DEFAULT_TOTAL_LIVES = 3;

        private int _totalLives;
        private int _currentLives;
        public BoxingMayhemLivesData() {
            Reset();
        }

        public void DeductCurrentLives() {
            _currentLives = UnityEngine.Mathf.Clamp(_currentLives - 1, 0, DEFAULT_TOTAL_LIVES);
        }
        public int GetCurrentLivesIndex() {
            return _totalLives - _currentLives;
        }
        public void CheckWhetherLivesLeft(out bool isLivesLeft) {
            isLivesLeft = _currentLives != 0;
        }
        public void Reset() {
            _currentLives = _totalLives = DEFAULT_TOTAL_LIVES;
        }
    }
}