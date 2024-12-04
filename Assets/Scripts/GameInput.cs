using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PlayerPrefsBindingsKey = "InputBindings";

    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlternate,
        GamepadPause
    }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnRebind;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PlayerPrefsBindingsKey))
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlayerPrefsBindingsKey));

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

    public string GetBindingText(Binding binding)
    {
        return binding switch
        {
            Binding.MoveUp => _playerInputActions.Player.Move.bindings[1].ToDisplayString(),
            Binding.MoveDown => _playerInputActions.Player.Move.bindings[2].ToDisplayString(),
            Binding.MoveLeft => _playerInputActions.Player.Move.bindings[3].ToDisplayString(),
            Binding.MoveRight => _playerInputActions.Player.Move.bindings[4].ToDisplayString(),
            Binding.Interact => _playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.InteractAlternate => _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            Binding.Pause => _playerInputActions.Player.Pause.bindings[0].ToDisplayString(),
            Binding.GamepadInteract => _playerInputActions.Player.Interact.bindings[1].ToDisplayString(),
            Binding.GamepadInteractAlternate => _playerInputActions.Player.InteractAlternate.bindings[1]
                .ToDisplayString(),
            Binding.GamepadPause => _playerInputActions.Player.Pause.bindings[1].ToDisplayString(),
            _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null)
        };
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            case Binding.MoveUp:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamepadInteract:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamepadInteractAlternate:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamepadPause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(binding), binding, null);
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            _playerInputActions.Player.Enable();
            onActionRebound();


            PlayerPrefs.SetString(PlayerPrefsBindingsKey, _playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}