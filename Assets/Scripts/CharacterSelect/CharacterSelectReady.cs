using System;
using System.Collections.Generic;
using SceneManagement;
using Unity.Netcode;
using UnityEngine;

namespace CharacterSelect
{
    public class CharacterSelectReady : NetworkBehaviour
    {
        public static CharacterSelectReady Instance { get; private set; }

        private readonly Dictionary<ulong, bool> _playersReady = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void SetPlayerReady()
        {
            SetPlayerReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            _playersReady[serverRpcParams.Receive.SenderClientId] = true;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
                if (!_playersReady.ContainsKey(clientId) || !_playersReady[clientId])
                    return;

            Loader.LoadNetworked(Loader.Scenes.GameScene);
        }
    }
}