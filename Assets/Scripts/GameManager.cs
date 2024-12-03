using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    [SerializeField] private float countdownToStartTime = 3f;
    [SerializeField] private float gamePlayingTime = 10f;

    private GameState _gameState;
    private float _waitingToStartTimer = 1f;
    private float _gamePlayingTimer;
    private float _countdownTimer;
    private bool _isGamePaused = false;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _countdownTimer = countdownToStartTime;
        _gamePlayingTimer = gamePlayingTime;

        ChangeState(GameState.WaitingToStart);
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
    }

    private void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }


    private void Update()
    {
        switch (_gameState)
        {
            case GameState.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (!(_waitingToStartTimer <= 0)) return;

                ChangeState(GameState.CountdownToStart);
                break;
            case GameState.CountdownToStart:
                _countdownTimer -= Time.deltaTime;
                if (!(_countdownTimer <= 0)) return;

                ChangeState(GameState.GamePlaying);
                break;
            case GameState.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (!(_gamePlayingTimer <= 0)) return;

                ChangeState(GameState.GameOver);
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeState(GameState newState)
    {
        _gameState = newState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;

        Time.timeScale = _isGamePaused ? 0f : 1f;

        if (_isGamePaused)
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        else
            OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePlaying()
    {
        return _gameState == GameState.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _gameState == GameState.CountdownToStart;
    }

    public float GetCountdownTimer()
    {
        return _countdownTimer;
    }

    public bool IsGameOver()
    {
        return _gameState == GameState.GameOver;
    }

    public float GetPlayingTimerNormalized()
    {
        return _gamePlayingTimer / gamePlayingTime;
    }
}