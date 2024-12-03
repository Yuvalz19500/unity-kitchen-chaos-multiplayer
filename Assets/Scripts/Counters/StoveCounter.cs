using System;
using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Counters
{
    public class StoveCounter : BaseCounter, IHasProgress
    {
        public enum StoveCounterState
        {
            Idle,
            Frying,
            Fried,
            Burned
        }

        [SerializeField] private StoveRecipeSO[] stoveRecipesSO;

        private float _fryingTimer;
        private float _burningTimer;
        private StoveRecipeSO _stoveRecipeSO;
        private StoveCounterState _currentStoveCounterState = StoveCounterState.Idle;

        public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
        public event EventHandler<OnStoveStateChangedArgs> OnStoveStateChanged;

        public class OnStoveStateChangedArgs : EventArgs
        {
            public StoveCounterState NewStoveCounterState;
        }

        private void Update()
        {
            if (!HasKitchenObject()) return;

            switch (_currentStoveCounterState)
            {
                case StoveCounterState.Idle:
                    break;
                case StoveCounterState.Frying:
                    _fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        ProgressNormalized = _fryingTimer / _stoveRecipeSO.fryingTimerMax
                    });

                    if (_fryingTimer >= _stoveRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_stoveRecipeSO.output, this);
                        _stoveRecipeSO = GetStoveRecipeSOForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());

                        ChangeStoveState(StoveCounterState.Fried);
                        _burningTimer = 0f;
                    }

                    break;
                case StoveCounterState.Fried:
                    if (!_stoveRecipeSO)
                    {
                        ChangeStoveState(StoveCounterState.Idle);
                        return;
                    }

                    _burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        ProgressNormalized = _burningTimer / _stoveRecipeSO.fryingTimerMax
                    });

                    if (_burningTimer >= _stoveRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_stoveRecipeSO.output, this);

                        ChangeStoveState(StoveCounterState.Burned);

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                        {
                            ProgressNormalized = 0f
                        });
                    }

                    break;
                case StoveCounterState.Burned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Interact(Player.Player player)
        {
            if (!HasKitchenObject())
            {
                if (!player.HasKitchenObject()) return;
                if (!HasFryingRecipeForKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO())) return;

                player.GetKitchenObject().SetKitchenObjectParent(this);
                _stoveRecipeSO = GetStoveRecipeSOForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());

                _fryingTimer = 0f;
                ChangeStoveState(StoveCounterState.Frying);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                {
                    ProgressNormalized = _fryingTimer / _stoveRecipeSO.fryingTimerMax
                });
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;
                    if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) return;

                    GetKitchenObject().DestroySelf();

                    ChangeStoveState(StoveCounterState.Idle);

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        ProgressNormalized = 0f
                    });
                }
                else
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                    ChangeStoveState(StoveCounterState.Idle);

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        ProgressNormalized = 0f
                    });
                }
            }
        }

        private bool HasFryingRecipeForKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
        {
            return GetStoveRecipeSOForKitchenObjectSO(inputKitchenObjectSO) != null;
        }

        private StoveRecipeSO GetStoveRecipeSOForKitchenObjectSO(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (StoveRecipeSO fryingRecipeSO in stoveRecipesSO)
                if (fryingRecipeSO.input == inputKitchenObjectSO)
                    return fryingRecipeSO;

            return null;
        }

        private void ChangeStoveState(StoveCounterState newStoveCounterState)
        {
            _currentStoveCounterState = newStoveCounterState;

            OnStoveStateChanged?.Invoke(this,
                new OnStoveStateChangedArgs { NewStoveCounterState = newStoveCounterState });
        }
    }
}