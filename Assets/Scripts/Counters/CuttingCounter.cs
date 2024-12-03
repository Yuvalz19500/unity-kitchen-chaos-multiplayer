using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        [SerializeField] private CuttingRecipeSO[] cuttingRecipesSO;

        private int _cuttingProgress;

        public static event EventHandler OnAnyCut;
        public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
        public event EventHandler OnCut;

        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                if (!player.HasKitchenObject()) return;

                if (!HasRecipeForKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO())) return;
                player.GetKitchenObject().SetKitchenObjectParent(this);
                SetCuttingProgress(0, GetCuttingRecipeSOForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO()));
            }
            else
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
                else
                {
                    if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }

        public override void InteractAlternate(Player player)
        {
            if (!HasKitchenObject() || !HasRecipeForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO())) return;

            CuttingRecipeSO cuttingRecipeSO =
                GetCuttingRecipeSOForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());
            SetCuttingProgress(_cuttingProgress + 1, cuttingRecipeSO);
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            if (_cuttingProgress < cuttingRecipeSO.requiredCuttingSteps) return;

            KitchenObjectSO outputKitchenObjectSO =
                GetOutputCutKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }

        private KitchenObjectSO GetOutputCutKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
        {
            return GetCuttingRecipeSOForKitchenObjectSO(inputKitchenObjectSO).output;
        }

        private bool HasRecipeForKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
        {
            return GetCuttingRecipeSOForKitchenObjectSO(inputKitchenObjectSO) != null;
        }

        private CuttingRecipeSO GetCuttingRecipeSOForKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipesSO)
            {
                if (cuttingRecipeSO.input == inputKitchenObjectSO)
                {
                    return cuttingRecipeSO;
                }
            }

            return null;
        }

        private void SetCuttingProgress(int progress, CuttingRecipeSO cuttingRecipeSO)
        {
            _cuttingProgress = progress;
            OnProgressChanged?.Invoke(this,
                new IHasProgress.OnProgressChangedArgs()
                    { ProgressNormalized = (float)progress / cuttingRecipeSO.requiredCuttingSteps });
        }
    }
}