using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeliveryManagerSingleOrderUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI orderNameText;
        [SerializeField] private GameObject iconsContainer;
        [SerializeField] private GameObject iconTemplate;

        public void SetOrderSO(OrderSO orderSO)
        {
            orderNameText.text = orderSO.orderName;

            foreach (Transform child in iconsContainer.transform)
            {
                if (child == iconTemplate.transform) continue;

                Destroy(child.gameObject);
            }

            foreach (KitchenObjectSO kitchenObjectSO in orderSO.kitchenObjectsSO)
            {
                GameObject iconGameObject = Instantiate(iconTemplate, iconsContainer.transform);
                iconGameObject.SetActive(true);
                iconGameObject.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
            }
        }
    }
}
