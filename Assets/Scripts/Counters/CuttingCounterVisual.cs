using System;
using UnityEngine;

namespace Counters
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        private static readonly int CutAnimId = Animator.StringToHash("Cut");

        [SerializeField] private CuttingCounter cuttingCounter;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            cuttingCounter.OnCut += CuttingCounterOnCut;
        }

        private void CuttingCounterOnCut(object sender, EventArgs e)
        {
            _animator.SetTrigger(CutAnimId);
        }
    }
}