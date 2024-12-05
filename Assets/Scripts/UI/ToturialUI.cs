using System;
using UnityEngine;

namespace UI
{
    public class ToturialUI : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            if (GameManager.Instance.IsCountdownToStartActive()) gameObject.SetActive(false);
        }
    }
}