using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class OrdersListSO : ScriptableObject
    {
        public List<OrderSO> ordersSO;
    }
}
