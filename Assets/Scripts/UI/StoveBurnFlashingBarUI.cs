using System;
using Counters;
using UnityEngine;

namespace UI
{
    public class StoveBurnFlashingBarUI : MonoBehaviour
    {
        private static readonly int IsFlashingAnimId = Animator.StringToHash("IsFlashing");
        [SerializeField] private StoveCounter stoveCounter;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
        }

        private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e)
        {
            _animator.SetBool(IsFlashingAnimId,
                stoveCounter.IsFried() && e.ProgressNormalized >= stoveCounter.GetWarningThreshold());
        }
    }
}