using System;

namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public static DeliveryCounter Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public override void Interact(Player.Player player)
        {
            if (!player.HasKitchenObject() ||
                !player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;

            DeliveryManager.Instance.DeliverOrder(plateKitchenObject);
            KitchenGameMultiplayer.Instance.DestroyKitchenObject(player.GetKitchenObject());
        }
    }
}