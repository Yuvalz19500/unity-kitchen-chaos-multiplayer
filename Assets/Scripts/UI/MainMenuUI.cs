using System;
using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(() => { Loader.Load(Loader.Scenes.GameScene); });
            quitButton.onClick.AddListener(Application.Quit);

            Time.timeScale = 1f;
        }
    }
}