using System;
using SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            resumeButton.onClick.AddListener(() => { GameManager.Instance.TogglePauseGame(); });
            mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scenes.MainMenuScene); });
            optionsButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                OptionsUI.Instance.Show(() =>
                {
                    gameObject.SetActive(true);
                    resumeButton.Select();
                });
            });
        }

        private void Start()
        {
            GameManager.Instance.OnLocalGamePaused += LocalGameManagerOnLocalGamePaused;
            GameManager.Instance.OnLocalGameResumed += LocalGameManagerOnLocalGameResumed;

            gameObject.SetActive(false);
        }

        private void LocalGameManagerOnLocalGameResumed(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void LocalGameManagerOnLocalGamePaused(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
            resumeButton.Select();
        }
    }
}