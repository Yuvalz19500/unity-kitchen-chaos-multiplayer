using System;
using UnityEngine;

namespace UI
{
    public class ConnectingUI : MonoBehaviour
    {
        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnTryingToJoinGame += KitchenGameMultiplayerOnTryingToJoinGame;
            KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayerOnFailedToJoinGame;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= KitchenGameMultiplayerOnTryingToJoinGame;
            KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayerOnFailedToJoinGame;
        }

        private void KitchenGameMultiplayerOnFailedToJoinGame(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void KitchenGameMultiplayerOnTryingToJoinGame(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
        }
    }
}