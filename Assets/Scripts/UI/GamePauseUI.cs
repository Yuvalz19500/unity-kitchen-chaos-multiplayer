using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            resumeButton.onClick.AddListener(() => { GameManager.Instance.TogglePauseGame(); });
            mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scenes.MainMenuScene); });
        }

        private void Start()
        {
            GameManager.Instance.OnGamePaused += GameManagerOnGamePaused;
            GameManager.Instance.OnGameResumed += GameManagerOnGameResumed;

            gameObject.SetActive(false);
        }

        private void GameManagerOnGameResumed(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void GameManagerOnGamePaused(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
        }
    }
}