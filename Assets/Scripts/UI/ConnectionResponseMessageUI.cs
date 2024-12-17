using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ConnectionResponseMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayerOnFailedToJoinGame;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayerOnFailedToJoinGame;
        }

        private void KitchenGameMultiplayerOnFailedToJoinGame(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
            messageText.text = NetworkManager.Singleton.DisconnectReason != ""
                ? NetworkManager.Singleton.DisconnectReason
                : "Failed to connect";
        }
    }
}