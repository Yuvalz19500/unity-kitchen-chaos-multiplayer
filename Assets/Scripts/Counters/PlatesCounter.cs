using System;
using ScriptableObjects;
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
            if (_platesCount >= maxPlatesCount || !GameManager.Instance.IsGamePlaying()) return;

            _spawnPlateTimer += Time.deltaTime;

            if (!(_spawnPlateTimer >= spawnPlateRate)) return;

            _spawnPlateTimer = 0f;

            _platesCount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObject()) return;
            if (_platesCount <= 0) return;

            _platesCount--;

            KitchenGameMultiplayer.Instance.SpawnKitchenObject(plateKitchenObjectSO, player);
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
        }
    }
}