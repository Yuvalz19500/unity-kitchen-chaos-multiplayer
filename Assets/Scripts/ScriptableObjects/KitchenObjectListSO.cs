using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class KitchenObjectListSO : ScriptableObject
    {
        public List<KitchenObjectSO> kitchenObjectsList;
    }
}