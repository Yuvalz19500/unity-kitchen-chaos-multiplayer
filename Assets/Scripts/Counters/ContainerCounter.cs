using System;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace Counters
{
    public class ContainerCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;
        public event EventHandler OnPlayerGrabbedObject;

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicServerRpc()
        {
            InteractLoginClientRpc();
        }

        [ClientRpc]
        private void InteractLoginClientRpc()
        {
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObject()) return;

            KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, player);

            InteractLogicServerRpc();
        }
    }
}