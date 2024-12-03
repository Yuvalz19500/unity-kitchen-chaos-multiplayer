using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += InteractOnPerformed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternateOnPerformed;
        _playerInputActions.Player.Pause.performed += PauseOnPerformed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= InteractOnPerformed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternateOnPerformed;
        _playerInputActions.Player.Pause.performed -= PauseOnPerformed;

        _playerInputActions.Dispose();
    }

    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternateOnPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractOnPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector.Normalize();
        return inputVector;
    }
}