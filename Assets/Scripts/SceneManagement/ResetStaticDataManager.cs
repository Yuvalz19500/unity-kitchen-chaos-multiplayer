using Counters;
using UnityEngine;

namespace SceneManagement
{
    public class ResetStaticDataManager : MonoBehaviour
    {
        private void Awake()
        {
            CuttingCounter.ResetStaticData();
            BaseCounter.ResetStaticData();
            TrashCounter.ResetStaticData();
        }
    }
}