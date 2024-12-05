using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestingNetcodeUI : MonoBehaviour
    {
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;

        private void Awake()
        {
            startHostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
                gameObject.SetActive(false);
            });
            startClientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
                gameObject.SetActive(false);
            });
        }
    }
}