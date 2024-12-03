using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipesDeliveredText;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

            gameObject.SetActive(false);
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            if (GameManager.Instance.IsGameOver())
                recipesDeliveredText.text = DeliveryManager.Instance.GetSucessfulOrders().ToString();

            gameObject.SetActive(GameManager.Instance.IsGameOver());
        }
    }
}