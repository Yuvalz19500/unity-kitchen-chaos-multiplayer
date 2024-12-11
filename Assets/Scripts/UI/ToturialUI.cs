using System;
using UnityEngine;

namespace UI
{
    public class ToturialUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnLocalPlayerReadyChanged += GameManagerOnLocalPlayerReadyChanged;
        }

        private void GameManagerOnLocalPlayerReadyChanged(object sender, EventArgs e)
        {
            if (GameManager.Instance.IsLocalPlayerReady()) gameObject.SetActive(false);
        }
    }
}