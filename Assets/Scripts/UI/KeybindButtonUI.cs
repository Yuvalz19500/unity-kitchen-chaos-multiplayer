using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KeybindButtonUI : MonoBehaviour
    {
        [SerializeField] private GameInput.Binding binding;
        [SerializeField] private GameInput.Binding gamepadBinding;
        [SerializeField] private TextMeshProUGUI keybindText;
        [SerializeField] private TextMeshProUGUI gamepadKeybindText;
        [SerializeField] private Button keybindButton;
        [SerializeField] private Button gamepadKeybindButton;
        [SerializeField] private bool hasGamepadBinding;

        public event EventHandler<OnKeybindButtonClickedEventArgs> OnKeybindButtonClicked;
        public event EventHandler<OnKeybindButtonClickedEventArgs> OnGamepadKeybindButtonClicked;

        public class OnKeybindButtonClickedEventArgs : EventArgs
        {
            public GameInput.Binding Binding;
            public Action OnRebind;
        }

        private void Awake()
        {
            if (!hasGamepadBinding) gamepadKeybindButton.gameObject.SetActive(false);

            keybindButton.onClick.AddListener(() =>
            {
                OnKeybindButtonClicked?.Invoke(this, new OnKeybindButtonClickedEventArgs
                {
                    Binding = binding,
                    OnRebind = UpdateVisual
                });
            });

            gamepadKeybindButton.onClick.AddListener(() =>
            {
                OnGamepadKeybindButtonClicked?.Invoke(this, new OnKeybindButtonClickedEventArgs
                {
                    Binding = gamepadBinding,
                    OnRebind = UpdateVisual
                });
            });
        }

        private void Start()
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            string keybindDisplayText = GameInput.Instance.GetBindingText(binding);
            string gamepadKeybindDisplayText = GameInput.Instance.GetBindingText(gamepadBinding);
            if (keybindDisplayText.Length > 3) keybindDisplayText = keybindDisplayText[..3];
            if (gamepadKeybindDisplayText.Length > 3) gamepadKeybindDisplayText = gamepadKeybindDisplayText[..3];

            keybindText.text = keybindDisplayText;
            gamepadKeybindText.text = gamepadKeybindDisplayText;
        }

        public bool IsGamepadBinding()
        {
            return hasGamepadBinding;
        }
    }
}