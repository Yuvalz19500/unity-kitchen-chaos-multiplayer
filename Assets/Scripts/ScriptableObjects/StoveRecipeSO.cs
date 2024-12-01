using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class StoveRecipeSO : ScriptableObject
    {
        public KitchenObjectSO input;
        public KitchenObjectSO output;
        public int fryingTimerMax;
    }
}
