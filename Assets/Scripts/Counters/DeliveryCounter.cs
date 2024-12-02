namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject() ||
                !player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;
            
            DeliveryManager.Instance.DeliverOrder(plateKitchenObject);
            player.GetKitchenObject().DestroySelf();
        }
    }
}
