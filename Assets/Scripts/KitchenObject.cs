using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter _clearCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (_clearCounter != null) _clearCounter.ClearKitchenObject();

        _clearCounter = clearCounter;

        if (_clearCounter.HasKitchenObject()) Debug.LogError("Counter already has a KitchenObject");

        _clearCounter.SetKitchenObject(this);

        transform.parent = _clearCounter.GetKitchenObjectFollowTransform();
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}