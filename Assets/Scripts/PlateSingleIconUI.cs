using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class PlateSingleIconUI : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}