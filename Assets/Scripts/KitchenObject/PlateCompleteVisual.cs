using System;
using ScriptableObjects;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private KitchenObjectSOGameObject[] kitchenObjectSOGameObjects;
  
    [Serializable]
    public struct KitchenObjectSOGameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSOGameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjects)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO != e.KitchenObjectSO) continue;
            
            kitchenObjectSOGameObject.gameObject.SetActive(true);
            break;
        }
    }
}
