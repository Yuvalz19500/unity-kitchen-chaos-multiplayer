using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountdownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!GameManager.Instance.IsCountdownToStartActive()) return;

            countdownText.text = Mathf.Ceil(GameManager.Instance.GetCountdownTimer()).ToString();
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            gameObject.SetActive(GameManager.Instance.IsCountdownToStartActive());
        }
    }
}