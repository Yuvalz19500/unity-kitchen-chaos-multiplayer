using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return;

        GameObject kitchenObject = Instantiate(kitchenObjectSO.prefab);
        kitchenObject.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}