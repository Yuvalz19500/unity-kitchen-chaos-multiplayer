using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class DeliveryManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject orderTemplate;

        private void Start()
        {
            DeliveryManager.Instance.OnOrderCreated += (sender, args) => UpdateVisual();
            DeliveryManager.Instance.OnOrderDelivered += (sender, args) => UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (Transform child in container.transform)
            {
                if (child == orderTemplate.transform) continue;

                Destroy(child.gameObject);
            }

            foreach (OrderSO orderSO in DeliveryManager.Instance.GetWaitingOrdersSO())
            {
                GameObject orderGameObject = Instantiate(orderTemplate, container.transform);
                orderGameObject.SetActive(true);
                orderGameObject.GetComponent<DeliveryManagerSingleOrderUI>().SetOrderSO(orderSO);
            }
        }
    }
}