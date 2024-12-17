using System;
using SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestingLobbyUI : MonoBehaviour
    {
        [SerializeField] private Button createGameButton;
        [SerializeField] private Button joinGameButton;

        private void Awake()
        {
            createGameButton.onClick.AddListener(() =>
            {
                KitchenGameMultiplayer.Instance.StartHost();
                Loader.LoadNetworked(Loader.Scenes.CharacterSelectScene);
            });
            joinGameButton.onClick.AddListener(() => { KitchenGameMultiplayer.Instance.StartClient(); });
        }
    }
}