using SceneManagement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HostDisconnectUI : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;

        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisconnectCallback;
            playAgainButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                Loader.Load(Loader.Scenes.MainMenuScene);
            });

            gameObject.SetActive(false);
        }

        private void NetworkManagerOnClientDisconnectCallback(ulong clientId)
        {
            if (clientId == NetworkManager.ServerClientId) gameObject.SetActive(true);
        }
    }
}