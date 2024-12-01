using UnityEngine;

namespace Counters
{
    public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        [SerializeField] private GameObject counterTopPoint;

        private KitchenObject _kitchenObject;

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

        public abstract void Interact(Player player);

        public virtual void InteractAlternate(Player player)
        {
        }
    }
}