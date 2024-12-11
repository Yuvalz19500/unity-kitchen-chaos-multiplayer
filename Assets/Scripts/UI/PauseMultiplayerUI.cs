using System;
using UnityEngine;

namespace UI
{
    public class PauseMultiplayerUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnLocalGamePaused += GameManagerOnLocalGamePaused;
            GameManager.Instance.OnLocalGameResumed += GameManagerOnLocalGameResumed;

            gameObject.SetActive(false);
        }

        private void GameManagerOnLocalGameResumed(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void GameManagerOnLocalGamePaused(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
        }
    }
}