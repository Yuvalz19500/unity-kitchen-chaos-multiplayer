using System;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private OrdersListSO possibleOrdersListSO;
    [SerializeField] private float spawnRecipeTimerMax = 4f;
    [SerializeField] private int maxOrders = 4;

    private readonly List<OrderSO> _waitingOrdersSO = new();
    private float _spawnRecipeTimer;
    private int _successfulOrders;

    public event EventHandler OnOrderCreated;
    public event EventHandler OnOrderDelivered;
    public event EventHandler OnOrderFailed;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _spawnRecipeTimer = spawnRecipeTimerMax;
    }

    private void Update()
    {
        if (!IsServer) return;

        if (_waitingOrdersSO.Count >= maxOrders || !GameManager.Instance.IsGamePlaying())
            return;

        _spawnRecipeTimer -= Time.deltaTime;
        if (!(_spawnRecipeTimer <= 0f)) return;


        _spawnRecipeTimer = spawnRecipeTimerMax;

        SpawnNewWaitingOrderClientRpc(Random.Range(0, possibleOrdersListSO.ordersSO.Count));
    }

    [ClientRpc]
    private void SpawnNewWaitingOrderClientRpc(int waitingOrderSOIndex)
    {
        OrderSO orderSO = possibleOrdersListSO.ordersSO[waitingOrderSOIndex];
        _waitingOrdersSO.Add(orderSO);
        OnOrderCreated?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverOrder(PlateKitchenObject plateKitchenObject)
    {
        for (int index = 0; index < _waitingOrdersSO.Count; index++)
        {
            OrderSO orderSO = _waitingOrdersSO[index];
            if (orderSO.kitchenObjectsSO.Count != plateKitchenObject.GetKitchenObjectsSOList().Count) continue;

            bool plateMatchesOrder = true;
            foreach (KitchenObjectSO orderKitchenObjectSO in orderSO.kitchenObjectsSO)
            {
                bool ingredientFound = false;
                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectsSOList())
                {
                    if (plateKitchenObjectSO != orderKitchenObjectSO) continue;

                    ingredientFound = true;
                    break;
                }

                if (ingredientFound) continue;

                plateMatchesOrder = false;
                break;
            }

            if (!plateMatchesOrder) continue;

            DeliverCorrectOrderServerRpc(index);
            return;
        }

        DeliverIncorrectOrderServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectOrderServerRpc()
    {
        DeliverIncorrectOrderClientRpc();
    }

    [ClientRpc]
    private void DeliverIncorrectOrderClientRpc()
    {
        OnOrderFailed?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectOrderServerRpc(int orderIndex)
    {
        DeliverCorrectOrderClientRpc(orderIndex);
    }

    [ClientRpc]
    private void DeliverCorrectOrderClientRpc(int orderIndex)
    {
        _waitingOrdersSO.RemoveAt(orderIndex);
        _successfulOrders++;

        OnOrderDelivered?.Invoke(this, EventArgs.Empty);
    }

    public List<OrderSO> GetWaitingOrdersSO()
    {
        return _waitingOrdersSO;
    }

    public int GetSucessfulOrders()
    {
        return _successfulOrders;
    }
}