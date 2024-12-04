using System;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        public static OptionsUI Instance { get; private set; }

        [SerializeField] private Button soundFXButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI soundFXText;
        [SerializeField] private TextMeshProUGUI musicText;

        private Action _onCloseOptions;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            soundFXButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.ChangeVolume();
                UpdateVisual();
            });

            musicButton.onClick.AddListener(() =>
            {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            });

            closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);

                _onCloseOptions();
            });

            gameObject.SetActive(false);
        }

        private void Start()
        {
            GameManager.Instance.OnGameResumed += GameManagerOnGameResumed;

            UpdateVisual();
        }

        private void GameManagerOnGameResumed(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisual()
        {
            soundFXText.text = "Sound Effect: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10);
            musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10);
        }

        public void Show(Action onCloseOptions)
        {
            _onCloseOptions = onCloseOptions;

            gameObject.SetActive(true);

            soundFXButton.Select();
        }
    }
}