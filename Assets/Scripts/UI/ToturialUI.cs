using System;
using UnityEngine;

namespace UI
{
    public class ToturialUI : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            if (GameManager.Instance.IsCountdownToStartActive()) gameObject.SetActive(false);
        }
    }
}