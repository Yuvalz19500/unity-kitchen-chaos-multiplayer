using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private GameObject counterTopPoint;

    private KitchenObject _kitchenObject;

    public void Interact()
    {
        if (_kitchenObject == null)
        {
            GameObject kitchenObject = Instantiate(kitchenObjectSO.prefab, counterTopPoint.transform.position,
                Quaternion.identity);

            kitchenObject.GetComponent<KitchenObject>().SetClearCounter(this);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint.transform;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}