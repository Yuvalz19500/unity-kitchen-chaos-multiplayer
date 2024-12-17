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
    private readonly NetworkVariable<bool> _isGamePaused = new();
    private readonly Dictionary<ulong, bool> _playersReady = new();
    private readonly Dictionary<ulong, bool> _playersPause = new();

    private bool _autoTestGamePauseState = false;
    private bool _isLocalGamePaused;
    private bool _isLocalPlayerReady;

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameResumed;
    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameResumed;

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
        _isGamePaused.OnValueChanged += IsGamePausedOnValueChanged;

        if (!IsServer) return;

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisconnectCallback;

        _gameState.Value = GameState.WaitingToStart;
        _gamePlayingTimer.Value = gamePlayingTime;
        _countdownTimer.Value = countdownToStartTime;
    }

    private void NetworkManagerOnClientDisconnectCallback(ulong clientId)
    {
        _autoTestGamePauseState = true;
    }

    private void IsGamePausedOnValueChanged(bool previousValue, bool newValue)
    {
        Time.timeScale = _isGamePaused.Value ? 0f : 1f;

        if (_isGamePaused.Value)
            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        else
            OnMultiplayerGameResumed?.Invoke(this, EventArgs.Empty);
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

    private void LateUpdate()
    {
        if (!_autoTestGamePauseState) return;

        TestGamePauseState();
        _autoTestGamePauseState = false;
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

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playersPause[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePauseState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playersPause[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePauseState();
    }

    private void TestGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            if (_playersPause.ContainsKey(clientId) && _playersPause[clientId])
            {
                _isGamePaused.Value = true;
                return;
            }

        _isGamePaused.Value = false;
    }

    public void TogglePauseGame()
    {
        _isLocalGamePaused = !_isLocalGamePaused;

        if (_isLocalGamePaused)
        {
            PauseGameServerRpc();
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();
            OnLocalGameResumed?.Invoke(this, EventArgs.Empty);
        }
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

    public bool IsWaitingToStart()
    {
        return _gameState.Value == GameState.WaitingToStart;
    }
}