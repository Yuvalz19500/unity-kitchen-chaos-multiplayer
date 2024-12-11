using System;
using UnityEngine;

namespace UI
{
    public class WaitingForOtherPlayersUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnLocalPlayerReadyChanged += GameManagerOnLocalPlayerReadyChanged;
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

            gameObject.SetActive(false);
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            if (GameManager.Instance.IsCountdownToStartActive()) gameObject.SetActive(false);
        }

        private void GameManagerOnLocalPlayerReadyChanged(object sender, EventArgs e)
        {
            if (GameManager.Instance.IsLocalPlayerReady()) gameObject.SetActive(true);
        }
    }
}