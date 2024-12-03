using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private OrdersListSO possibleOrdersListSO;
    [SerializeField] private float spawnRecipeTimerMax = 4f;
    [SerializeField] private int maxOrders = 4;

    private readonly List<OrderSO> _waitingOrdersSO = new();
    private float _spawnRecipeTimer;
    private int sucessfulOrders;

    public event EventHandler OnOrderCreated;
    public event EventHandler OnOrderDelivered;
    public event EventHandler OnOrderFailed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        if (_waitingOrdersSO.Count >= maxOrders) return;

        _spawnRecipeTimer -= Time.deltaTime;
        if (!(_spawnRecipeTimer <= 0f)) return;

        _spawnRecipeTimer = spawnRecipeTimerMax;

        OrderSO newOrder = possibleOrdersListSO.ordersSO[Random.Range(0, possibleOrdersListSO.ordersSO.Count)];
        _waitingOrdersSO.Add(newOrder);
        OnOrderCreated?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverOrder(PlateKitchenObject plateKitchenObject)
    {
        foreach (OrderSO orderSO in _waitingOrdersSO)
        {
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

            _waitingOrdersSO.Remove(orderSO);
            sucessfulOrders++;

            OnOrderDelivered?.Invoke(this, EventArgs.Empty);
            return;
        }

        OnOrderFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<OrderSO> GetWaitingOrdersSO()
    {
        return _waitingOrdersSO;
    }

    public int GetSucessfulOrders()
    {
        return sucessfulOrders;
    }
}