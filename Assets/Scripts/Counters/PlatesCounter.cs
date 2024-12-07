using System;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace Counters
{
    public class PlatesCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
        [SerializeField] private float spawnPlateRate = 4f;
        [SerializeField] private int maxPlatesCount = 4;

        private float _spawnPlateTimer;
        private int _platesCount;

        public event EventHandler OnPlateSpawned;
        public event EventHandler OnPlateTaken;

        private void Update()
        {
            if (!IsServer) return;

            if (_platesCount >= maxPlatesCount || !GameManager.Instance.IsGamePlaying()) return;

            _spawnPlateTimer += Time.deltaTime;

            if (!(_spawnPlateTimer >= spawnPlateRate)) return;

            _spawnPlateTimer = 0f;
            SpawnPlateServerRpc();
        }

        [ServerRpc]
        private void SpawnPlateServerRpc()
        {
            SpawnPlateClientRpc();
        }

        [ClientRpc]
        private void SpawnPlateClientRpc()
        {
            _platesCount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicServerRpc()
        {
            InteractLoginClientRpc();
        }

        [ClientRpc]
        private void InteractLoginClientRpc()
        {
            _platesCount--;
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObject()) return;
            if (_platesCount <= 0) return;

            KitchenGameMultiplayer.Instance.SpawnKitchenObject(plateKitchenObjectSO, player);

            InteractLogicServerRpc();
        }
    }
}