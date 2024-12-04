using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ToturialKeybindItemUI : MonoBehaviour
    {
        [SerializeField] private GameInput.Binding binding;
        [SerializeField] private TextMeshProUGUI keybindText;
        [SerializeField] private bool staticText;
        [SerializeField] private bool shortenText;

        private void Start()
        {
            if (staticText) return;

            GameInput.Instance.OnRebind += GameInputOnRebind;

            UpdateVisual();
        }

        private void GameInputOnRebind(object sender, EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            string keybindDisplayText = GameInput.Instance.GetBindingText(binding);
            if (keybindDisplayText.Length > 3 && shortenText) keybindDisplayText = keybindDisplayText[..3];

            keybindText.text = keybindDisplayText;
        }
    }
}