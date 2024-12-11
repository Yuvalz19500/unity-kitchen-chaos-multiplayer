using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
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
    [SerializeField] private float gamePlayingTime = 300f;

    private readonly NetworkVariable<GameState> _gameState = new();
    private readonly NetworkVariable<float> _gamePlayingTimer = new();
    private readonly NetworkVariable<float> _countdownTimer = new();
    private bool _isGamePaused;
    private bool _isLocalPlayerReady;
    private readonly Dictionary<ulong, bool> _playersReady = new();

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;
    public event EventHandler OnLocalPlayerReadyChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (!IsServer) return;

        _countdownTimer.Value = countdownToStartTime;
        _gamePlayingTimer.Value = gamePlayingTime;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _gameState.OnValueChanged += GameStateOnValueChanged;

        if (!IsServer) return;

        _gameState.Value = GameState.WaitingToStart;
        _gamePlayingTimer.Value = gamePlayingTime;
        _countdownTimer.Value = countdownToStartTime;
    }

    private void GameStateOnValueChanged(GameState previousValue, GameState newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_gameState.Value != GameState.WaitingToStart) return;

        _isLocalPlayerReady = true;
        OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playersReady[serverRpcParams.Receive.SenderClientId] = true;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            if (!_playersReady.ContainsKey(clientId) || !_playersReady[clientId])
                return;

        _gameState.Value = GameState.CountdownToStart;
    }

    private void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }


    private void Update()
    {
        if (!IsServer) return;

        switch (_gameState.Value)
        {
            case GameState.WaitingToStart:
                break;
            case GameState.CountdownToStart:
                _countdownTimer.Value -= Time.deltaTime;
                if (!(_countdownTimer.Value <= 0)) return;

                _gameState.Value = GameState.GamePlaying;
                break;
            case GameState.GamePlaying:
                _gamePlayingTimer.Value -= Time.deltaTime;
                if (!(_gamePlayingTimer.Value <= 0)) return;

                _gameState.Value = GameState.GameOver;
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
        return _gameState.Value == GameState.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _gameState.Value == GameState.CountdownToStart;
    }

    public float GetCountdownTimer()
    {
        return _countdownTimer.Value;
    }

    public bool IsGameOver()
    {
        return _gameState.Value == GameState.GameOver;
    }

    public float GetPlayingTimerNormalized()
    {
        return _gamePlayingTimer.Value / gamePlayingTime;
    }

    public bool IsLocalPlayerReady()
    {
        return _isLocalPlayerReady;
    }
}