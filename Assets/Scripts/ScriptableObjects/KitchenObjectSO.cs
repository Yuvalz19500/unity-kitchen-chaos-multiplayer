using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class KitchenObjectSO : ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
        public string objectName;
    }
}