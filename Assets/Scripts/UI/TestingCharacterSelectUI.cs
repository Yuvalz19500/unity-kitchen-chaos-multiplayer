using System;
using CharacterSelect;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestingCharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button readyButton;

        private void Awake()
        {
            readyButton.onClick.AddListener(() => { CharacterSelectReady.Instance.SetPlayerReady(); });
        }
    }
}