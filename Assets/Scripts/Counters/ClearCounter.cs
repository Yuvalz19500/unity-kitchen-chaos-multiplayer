namespace Counters
{
    public class ClearCounter : BaseCounter
    {
        public override void Interact(Player.Player player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                    player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
                else
                {
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                            KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                    }
                    else
                    {
                        if (!GetKitchenObject().TryGetPlate(out plateKitchenObject)) return;

                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                            KitchenGameMultiplayer.Instance.DestroyKitchenObject(player.GetKitchenObject());
                    }
                }
            }
        }
    }
}