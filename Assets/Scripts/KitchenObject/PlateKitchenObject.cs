using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList; 
    
    private readonly List<KitchenObjectSO> _kitchenObjectsSOList = new();
    
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }
    
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO)) return false;
        if(_kitchenObjectsSOList.Contains(kitchenObjectSO)) return false;
        
        _kitchenObjectsSOList.Add(kitchenObjectSO);
        
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { KitchenObjectSO = kitchenObjectSO });
        return true;
    }
    
    public List<KitchenObjectSO> GetKitchenObjectsSOList()
    {
        return _kitchenObjectsSOList;
    }
}
