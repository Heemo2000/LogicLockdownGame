using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Game.Controls;
namespace Game.Core
{
    public enum GamePauseStatus
    {
        Paused,
        UnPaused
    }

    public enum GameplayStatus
    {
        None,
        OnGoing,
        End
    }
    public class GameManager : GenericSingleton<GameManager>
    {

        [SerializeField]private Button resumeBtn;
        public UnityEvent OnSceneStart;
        public UnityEvent OnGamePaused;
        public UnityEvent OnGameResumed;

        public UnityEvent OnGameplayStart;
        public UnityEvent OnGameEnd;
        public UnityEvent OnBackToMain;

        private GamePauseStatus _gamePauseStatus = GamePauseStatus.UnPaused;
        private GameplayStatus _gameplayStatus = GameplayStatus.None;
        private PauseControls _pauseControls;

        private Button _resumeBtn;

        public GamePauseStatus GamePauseStatus { get=> _gamePauseStatus; }
        public GameplayStatus GameplayStatus { get=> _gameplayStatus;}

        private void PauseGame()
        {
            Time.timeScale = 0.0f;
            _gamePauseStatus = GamePauseStatus.Paused;
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1.0f;
            _gamePauseStatus = GamePauseStatus.UnPaused;
        }

        private void StartGame()
        {
            _gameplayStatus = GameplayStatus.OnGoing;
        }

        private void EndGame()
        {
            _gameplayStatus = GameplayStatus.End;
        }

        private void ExitGameplay()
        {
            _gameplayStatus = GameplayStatus.None;
        }

        private void CheckPause(InputAction.CallbackContext context)
        {
            Debug.Log("Checking pause status");
            if(_gamePauseStatus == GamePauseStatus.UnPaused)
            {
                OnGamePaused?.Invoke();
            }
            else
            {
                OnGameResumed?.Invoke();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _pauseControls = new PauseControls();
        }
        private void Start() 
        {
            OnGamePaused.AddListener(PauseGame);
            OnGameResumed.AddListener(UnpauseGame);    
            OnGameplayStart.AddListener(StartGame);
            OnGameEnd.AddListener(EndGame);
            OnBackToMain.AddListener(ExitGameplay);

            resumeBtn.onClick.AddListener(()=> OnGameResumed?.Invoke());

            _pauseControls.PauseActionMap.PauseClick.Enable();
            _pauseControls.PauseActionMap.PauseClick.performed += CheckPause;
            Time.timeScale = 1.0f;
        }

        private void OnEnable() 
        {
            _pauseControls.PauseActionMap.PauseClick.Enable();    
        }

        private void OnDisable() 
        {
            _pauseControls.PauseActionMap.PauseClick.Disable();    
        }

        private void OnDestroy() {
            resumeBtn.onClick.RemoveAllListeners();
            _pauseControls.PauseActionMap.PauseClick.performed -= CheckPause;
        }        
    }

}
