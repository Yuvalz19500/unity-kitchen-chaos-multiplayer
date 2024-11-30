using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipesSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject()) return;
            
            if (HasRecipeForKitchenObject(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject() || !HasRecipeForKitchenObject(GetKitchenObject().GetKitchenObjectSO())) return;

        KitchenObjectSO outputKitchenObjectSO = GetOutputCutKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());
        GetKitchenObject().DestroySelf();

        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
    }

    private KitchenObjectSO GetOutputCutKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipesSO)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }

        return null;
    }

    private bool HasRecipeForKitchenObject(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipesSO)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }

        return false;
    }
}