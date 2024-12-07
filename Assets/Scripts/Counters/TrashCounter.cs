using System;
using Unity.Netcode;

namespace Counters
{
    public class TrashCounter : BaseCounter
    {
        public static event EventHandler OnAnyObjectTrashed;

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicServerRpc()
        {
            InteractLoginClientRpc();
        }

        [ClientRpc]
        private void InteractLoginClientRpc()
        {
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }

        public new static void ResetStaticData()
        {
            OnAnyObjectTrashed = null;
        }

        public override void Interact(Player.Player player)
        {
            if (!player.HasKitchenObject()) return;

            KitchenGameMultiplayer.Instance.DestroyKitchenObject(player.GetKitchenObject());

            InteractLogicServerRpc();
        }
    }
}