using System;
using UnityEngine;

namespace UI
{
    public class PauseMultiplayerUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnMultiplayerGamePaused += GameManagerOnMultiplayerGamePaused;
            GameManager.Instance.OnMultiplayerGameResumed += GameManagerOnMultiplayerGameResumed;

            gameObject.SetActive(false);
        }

        private void GameManagerOnMultiplayerGameResumed(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void GameManagerOnMultiplayerGamePaused(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
        }
    }
}