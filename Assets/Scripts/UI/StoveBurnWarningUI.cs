using System;
using Counters;
using UnityEngine;

namespace UI
{
    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private void Start()
        {
            stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;

            gameObject.SetActive(false);
        }

        private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e)
        {
            gameObject.SetActive(stoveCounter.IsFried() && e.ProgressNormalized >= stoveCounter.GetWarningThreshold());
        }
    }
}