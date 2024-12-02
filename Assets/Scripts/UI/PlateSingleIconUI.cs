using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlateSingleIconUI : MonoBehaviour
    {
        [SerializeField] private Image image;
    
        public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
        {
            image.sprite = kitchenObjectSO.sprite;
        }
    }
}
