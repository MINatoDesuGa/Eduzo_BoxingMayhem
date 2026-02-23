using Eduzo.Games.BoxingMayhem.Data;
using System;
using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.UI {
    public class BoxingMayhemQuizTimerHandler : MonoBehaviour, Interfaces.ISaveable {
        public static event Action<Controller.GameOverType> OnBoxingMayhemQuizTimerUp;

        private const float TIME_LIMIT_PER_QUESTION = 5f;
        private const int DELAY_TIME = 1;

        [Header("UI Elements")]
        [SerializeField] private TMPro.TMP_Text _timerText;
        [SerializeField] private Utility.CanvasGroupToggle _timerCanvasGroupToggle;

        [SerializeField] private int _timeLimit = 10;
        private int _timeRemaining;

        private Coroutine _timerCoroutine;
        private WaitForSeconds _timerDelay = new(DELAY_TIME);

        public BoxingMayhemSaveDataHandler SaveDataHandler { get; private set; }
        #region Mono
        private void OnEnable() {
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemGameOver += OnGameOver;
            Controller.BoxingMayhemGameFlowHandler.OnSaveDataHandlerInit += InitializeSaveDataHandler;

            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
        }
        private void OnDisable() {
            Controller.BoxingMayhemGameFlowHandler.OnBoxingMayhemGameOver -= OnGameOver;
            Controller.BoxingMayhemGameFlowHandler.OnSaveDataHandlerInit -= InitializeSaveDataHandler;

            BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected -= OnTestModeSelected;
            BoxingMayhemGameOverUIManager.OnBoxingMayhemHome -= OnQuitToHome;
        }
        #endregion
        #region Event Listeners
        private void OnGameOver(Controller.GameOverType gameOverType, float score) {
            StopTimer();
            ///Saving
            Save(SaveType.ActiveTime, _timeLimit - _timeRemaining);
            ///
            if (!_timerCanvasGroupToggle.IsVisible()) return;
            _timerCanvasGroupToggle.Toggle();
        }
        private void OnTestModeSelected() {
            ActivateInGameEventListening(true);
            StartTimer();
        }
        private void OnQuitToHome() {
            if (Controller.BoxingMayhemGameFlowHandler.CurrentGameMode is not Controller.GameMode.Test) return;

            ActivateInGameEventListening(false);
        }
        private void StartTimer() {
            // print("starting timer");
            ResetTimer();
            StopTimer();
            _timerCoroutine = StartCoroutine(RunTimer());

            if (_timerCanvasGroupToggle.IsVisible()) return;
            _timerCanvasGroupToggle.Toggle();
        }
        #endregion
        private void ActivateInGameEventListening(bool active) {
            if (active) {
                BoxingMayhemGameOverUIManager.OnBoxingMayhemReplay += StartTimer;
            } else {
                BoxingMayhemGameOverUIManager.OnBoxingMayhemReplay -= StartTimer;
            }
        }
        private void StopTimer() {
            if (_timerCoroutine != null) {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }
        private void ResetTimer() {
            _timeRemaining = _timeLimit;

            UpdateTimerUi(_timeLimit);
        }
        private void OnTimeUp() {
            OnBoxingMayhemQuizTimerUp?.Invoke(Controller.GameOverType.TimeUp);
            ResetTimer();
            //Debug.Log("Time's up!");
        }
        private System.Collections.IEnumerator RunTimer() {
            while (_timeRemaining > 0) {
                UpdateTimerUi(Mathf.CeilToInt(_timeRemaining));
                _timeRemaining -= DELAY_TIME;
                yield return _timerDelay;
            }
            _timeRemaining = 0;
            OnTimeUp();
        }
        private void UpdateTimerUi(int currentTime) {
            var minutes = currentTime / 60;
            var seconds = currentTime % 60;
            if (minutes > 0) {
                _timerText.SetText($"<sprite=0> {minutes:D2}:{seconds:D2}");
            } else {
                _timerText.SetText($"<sprite=0> {seconds:D2}");
            }
        }
        #region ISaveable Implementation
        public void InitializeSaveDataHandler(BoxingMayhemSaveDataHandler saveDataHandler) {
            SaveDataHandler = saveDataHandler;
        }

        public void Save(SaveType saveType, object data) {
            switch (saveType) {
                case SaveType.ActiveTime:
                    SaveDataHandler.UpdateSaveData(SaveType.ActiveTime, data);
                    break;
            }
        }
        #endregion
    }
}