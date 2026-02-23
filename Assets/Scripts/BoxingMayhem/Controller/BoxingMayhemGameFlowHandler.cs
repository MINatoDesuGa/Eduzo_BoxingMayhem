using Eduzo.Games.BoxingMayhem.Sound;
using System;
using UnityEngine;

namespace Eduzo.Games.BoxingMayhem.Controller {
    public class BoxingMayhemGameFlowHandler : MonoBehaviour, Interfaces.ISaveable {
        private const float SAVE_DELAY = 1f;

        public static event Action<Data.BoxingMayhemSaveDataHandler> OnSaveDataHandlerInit;
        public static event Action<bool> OnBoxingMayhemAnswerValidated;
        public static event Action<string> OnBoxingMayhemQuestionUpdate;
        public static event Action<GameOverType, float> OnBoxingMayhemGameOver;
        public static GameMode CurrentGameMode { get; private set; }


        private GameState _currentGameState;

        private Data.BoxingMayhemQuestionFormData _questionFormData;
        private Data.BoxingMayhemScoreData _scoreData;
        public Data.BoxingMayhemSaveDataHandler SaveDataHandler { get; private set; }
        #region Mono
        private void OnEnable() {
            UI.BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit += OnQuestionsInit;
            UI.BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected += OnPracticeModeSelected;
            UI.BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected += OnTestModeSelected;
            UI.BoxingMayhemPunchBagUIManager.OnBoxingMayhemPunchBagClick += OnPunchBagClicked;
            UI.BoxingMayhemQuizTimerHandler.OnBoxingMayhemQuizTimerUp += HandleGameOver;
            UI.BoxingMayhemLivesUIManager.OnBoxingMayhemLivesUp += HandleGameOver;
            UI.BoxingMayhemGameOverUIManager.OnBoxingMayhemReplay += OnGameReplay;
            UI.BoxingMayhemGameOverUIManager.OnBoxingMayhemHome += OnQuitToHome;
        }
        private void OnDisable() {
            UI.BoxingMayhemQuestionFormUIManager.OnBoxingMayhemQuestionInit -= OnQuestionsInit;
            UI.BoxingMayhemMenuUIManager.OnBoxingMayhemPracticeModeSelected -= OnPracticeModeSelected;
            UI.BoxingMayhemMenuUIManager.OnBoxingMayhemTestModeSelected -= OnTestModeSelected;
            UI.BoxingMayhemPunchBagUIManager.OnBoxingMayhemPunchBagClick -= OnPunchBagClicked;
            UI.BoxingMayhemQuizTimerHandler.OnBoxingMayhemQuizTimerUp -= HandleGameOver;
            UI.BoxingMayhemLivesUIManager.OnBoxingMayhemLivesUp -= HandleGameOver;
            UI.BoxingMayhemGameOverUIManager.OnBoxingMayhemReplay -= OnGameReplay;
            UI.BoxingMayhemGameOverUIManager.OnBoxingMayhemHome -= OnQuitToHome;
        }
        #endregion

        #region Event Listeners
        private void OnQuestionsInit(System.Collections.Generic.List<Data.QuestionData> questionCollection) {
            _questionFormData = new(questionCollection);
            _scoreData = new(_questionFormData.GetTotalQuestions());
            InitializeSaveDataHandler(new Data.BoxingMayhemSaveDataHandler());
        }
        private void OnPracticeModeSelected() {
            ResetGame();

            BoxingMayhemSoundManager.Instance.PlayMusic(BoxingMayhemSoundManager.SoundId.BGMusic);

            OnBoxingMayhemQuestionUpdate?.Invoke(_questionFormData.GetCurrentQuestionData().Question);
        }
        private void OnTestModeSelected() {
            ResetGame();

            BoxingMayhemSoundManager.Instance.PlayMusic(BoxingMayhemSoundManager.SoundId.BGMusic);

            OnBoxingMayhemQuestionUpdate?.Invoke(_questionFormData.GetCurrentQuestionData().Question);
        }
        private void OnPunchBagClicked(bool isTrueAnswerBag) {
            ValidateAnswer(isTrueAnswerBag);

            LoadNextQuestion();
        }
        private void OnGameReplay() {
            UpdateGameState(GameState.Playing);
            ResetGame();
            BoxingMayhemSoundManager.Instance.PlayMusic(BoxingMayhemSoundManager.SoundId.BGMusic);
            OnBoxingMayhemQuestionUpdate.Invoke(_questionFormData.GetCurrentQuestionData().Question);
        }
        private void OnQuitToHome() {
            UpdateGameState(GameState.MainMenu);
            ResetGame();
            BoxingMayhemSoundManager.Instance.StopMusic();
        }
        #endregion
        private void ValidateAnswer(bool userAnswer) {
            var correctAnswer = _questionFormData.GetCurrentQuestionData().Answer;
            var isAnswerCorrect = correctAnswer == userAnswer;

            ///Update User Response Data in Save///
            SaveDataHandler.UpdateQuestionResponseData(_questionFormData.GetCurrentQuestionData().Question, userAnswer, correctAnswer);
            Save(Data.SaveType.ResponseData);
            ///-------------------------///

            if (isAnswerCorrect) {
                BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.CorrectAnswer);
                _scoreData.UpdateCorrectAnswerCount();
            } else {
                BoxingMayhemSoundManager.Instance.PlaySFX(BoxingMayhemSoundManager.SoundId.IncorrectAnswer);
            }
            OnBoxingMayhemAnswerValidated?.Invoke(isAnswerCorrect);
        }
        private void LoadNextQuestion() {
            _questionFormData.UpdateCurrentQuestionIndex();
            _questionFormData.CheckWhetherQuestionCompleted(out bool isQuestionOver);
            if (!isQuestionOver) {
                OnBoxingMayhemQuestionUpdate?.Invoke(_questionFormData.GetCurrentQuestionData().Question);
                return;
            }
            HandleGameOver(GameOverType.QuestionCompleted);
        }
        private void HandleGameOver(GameOverType gameOverType) {
            BoxingMayhemSoundManager.Instance.StopMusic();

            _scoreData.CalculateScore(out float totalScore);
            switch (CurrentGameMode) {
                case GameMode.Test:
                    ///Update Score and invoke saving///
                    Save(Data.SaveType.Score, totalScore);
                    Invoke(nameof(DelayInvokeSaveResult), SAVE_DELAY);
                    ///------------------------------///
                    OnBoxingMayhemGameOver?.Invoke(gameOverType, totalScore);
                    UpdateGameState(GameState.GameOver);
                    break;
                case GameMode.Practice:
                    _scoreData.IsFullScoreObtained(out bool isFullScore);
                    if (!isFullScore) {
                        OnGameReplay();
                        break;
                    }
                    OnBoxingMayhemGameOver?.Invoke(gameOverType, totalScore);
                    UpdateGameState(GameState.GameOver);
                    break;
            }
        }
        private void UpdateGameState(GameState gameState) {
            _currentGameState = gameState;
        }
        public static void UpdateGameMode(GameMode gameMode) {
            CurrentGameMode = gameMode;
        }
        private void ResetGame() {
            _scoreData.Reset();
            _questionFormData.Reset();
            SaveDataHandler.ResetData();
        }
        #region ISaveable Implementation 
        public void InitializeSaveDataHandler(Data.BoxingMayhemSaveDataHandler saveDataHandler) {
            SaveDataHandler = saveDataHandler;
            OnSaveDataHandlerInit?.Invoke(SaveDataHandler);
        }
        public void Save(Data.SaveType saveType, object data = null) {
            switch(saveType) {
                case Data.SaveType.Score:
                    SaveDataHandler.UpdateSaveData(Data.SaveType.Score, data);
                    break;
                case Data.SaveType.ResponseData:
                    SaveDataHandler.UpdateSaveData(Data.SaveType.ResponseData);
                    break;
            }
        }
        private void DelayInvokeSaveResult() {
            SaveDataHandler.SaveResult();
        }
        #endregion
    }
    public enum GameState {
        Playing, MainMenu, GameOver
    }
    public enum GameMode {
        Practice, Test
    }
    public enum GameOverType {
        TimeUp, QuestionCompleted, LivesUp
    }
}