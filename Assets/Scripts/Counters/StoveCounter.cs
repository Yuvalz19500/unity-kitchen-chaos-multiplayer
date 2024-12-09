using System;
using ScriptableObjects;
using UI;
using Unity.Netcode;
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
        [SerializeField] private float warningThreshold = 0.5f;

        private readonly NetworkVariable<float> _fryingTimer = new(0f);
        private readonly NetworkVariable<float> _burningTimer = new(0f);
        private StoveRecipeSO _stoveRecipeSO;
        private readonly NetworkVariable<StoveCounterState> _currentStoveCounterState = new(StoveCounterState.Idle);

        public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
        public event EventHandler<OnStoveStateChangedArgs> OnStoveStateChanged;

        public class OnStoveStateChangedArgs : EventArgs
        {
            public StoveCounterState NewStoveCounterState;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _fryingTimer.OnValueChanged += FryingTimerOnValueChanged;
            _burningTimer.OnValueChanged += BurningTimerOnValueChanged;
            _currentStoveCounterState.OnValueChanged += CurrentStoveCounterStateOnValueChanged;
        }

        private void CurrentStoveCounterStateOnValueChanged(StoveCounterState previousValue, StoveCounterState newValue)
        {
            OnStoveStateChanged?.Invoke(this,
                new OnStoveStateChangedArgs { NewStoveCounterState = _currentStoveCounterState.Value });

            if (_currentStoveCounterState.Value is StoveCounterState.Burned or StoveCounterState.Idle)
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                {
                    ProgressNormalized = 0f
                });
        }

        private void BurningTimerOnValueChanged(float previousValue, float newValue)
        {
            float burningTimerMax = _stoveRecipeSO ? _stoveRecipeSO.fryingTimerMax : 1f;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
            {
                ProgressNormalized = _burningTimer.Value / burningTimerMax
            });
        }

        private void FryingTimerOnValueChanged(float previousValue, float newValue)
        {
            float fryingTimerMax = _stoveRecipeSO ? _stoveRecipeSO.fryingTimerMax : 1f;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
            {
                ProgressNormalized = _fryingTimer.Value / fryingTimerMax
            });
        }

        private void Update()
        {
            if (!IsServer) return;
            if (!HasKitchenObject()) return;

            switch (_currentStoveCounterState.Value)
            {
                case StoveCounterState.Idle:
                    break;
                case StoveCounterState.Frying:
                    _fryingTimer.Value += Time.deltaTime;

                    if (_fryingTimer.Value >= _stoveRecipeSO.fryingTimerMax)
                    {
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());

                        KitchenGameMultiplayer.Instance.SpawnKitchenObject(_stoveRecipeSO.output, this);
                        SetStoveRecipeSOClientRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject()
                                .GetKitchenObjectSO()));

                        _currentStoveCounterState.Value = StoveCounterState.Fried;
                        _burningTimer.Value = 0f;
                    }

                    break;
                case StoveCounterState.Fried:
                    if (!_stoveRecipeSO)
                    {
                        _currentStoveCounterState.Value = StoveCounterState.Idle;
                        return;
                    }

                    _burningTimer.Value += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs
                    {
                        ProgressNormalized = _burningTimer.Value / _stoveRecipeSO.fryingTimerMax
                    });

                    if (_burningTimer.Value >= _stoveRecipeSO.fryingTimerMax)
                    {
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());

                        KitchenGameMultiplayer.Instance.SpawnKitchenObject(_stoveRecipeSO.output, this);

                        _currentStoveCounterState.Value = StoveCounterState.Burned;
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

                KitchenObject kitchenObject = player.GetKitchenObject();
                kitchenObject.SetKitchenObjectParent(this);
                InteractLogicPlaceObjectServerRpc(
                    KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO()));
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;
                    if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) return;

                    KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());

                    SetStateIdleServerRpc();
                }
                else
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                    SetStateIdleServerRpc();
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetStateIdleServerRpc()
        {
            _currentStoveCounterState.Value = StoveCounterState.Idle;
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicPlaceObjectServerRpc(int kitchenObjectSOIndex)
        {
            _fryingTimer.Value = 0f;
            _currentStoveCounterState.Value = StoveCounterState.Frying;

            SetStoveRecipeSOClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        private void SetStoveRecipeSOClientRpc(int kitchenObjectSOIndex)
        {
            _stoveRecipeSO =
                GetStoveRecipeSOForKitchenObjectSO(
                    KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex));
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

        public bool IsFried()
        {
            return _currentStoveCounterState.Value == StoveCounterState.Fried;
        }

        public float GetWarningThreshold()
        {
            return warningThreshold;
        }
    }
}