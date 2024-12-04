using System;
using UnityEngine;

namespace UI
{
    public class KeybindsContainerUI : MonoBehaviour
    {
        [SerializeField] private KeybindButtonUI[] keybindButtons;
        [SerializeField] private GameObject pressAnyKeyToRebindContainerGameObject;

        private void Awake()
        {
            foreach (KeybindButtonUI keybindButton in keybindButtons)
            {
                keybindButton.OnKeybindButtonClicked += KeybindButtonOnKeybindButtonClicked;

                if (keybindButton.IsGamepadBinding())
                    keybindButton.OnGamepadKeybindButtonClicked += KeybindButtonOnKeybindButtonClicked;
            }
        }

        private void KeybindButtonOnKeybindButtonClicked(object sender,
            KeybindButtonUI.OnKeybindButtonClickedEventArgs e)
        {
            pressAnyKeyToRebindContainerGameObject.SetActive(true);

            GameInput.Instance.RebindBinding(e.Binding, () =>
            {
                pressAnyKeyToRebindContainerGameObject.SetActive(false);
                e.OnRebind();
            });
        }
    }
}