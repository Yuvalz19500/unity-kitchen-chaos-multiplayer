using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class OrderSO : ScriptableObject
    {
        public List<KitchenObjectSO> kitchenObjectsSO;
        public string orderName;
    }
}
