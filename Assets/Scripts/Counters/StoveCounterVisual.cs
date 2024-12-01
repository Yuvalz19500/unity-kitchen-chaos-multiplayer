using UnityEngine;

namespace Counters
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        [SerializeField] private GameObject stoveOnGameObject;
        [SerializeField] private GameObject particlesGameObject;

        private void Start()
        {
            stoveCounter.OnStoveStateChanged += StoveCounterOnStoveStateChanged;
        }

        private void StoveCounterOnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedArgs e)
        {
            bool showVisual = e.NewStoveCounterState is StoveCounter.StoveCounterState.Frying or StoveCounter.StoveCounterState.Fried;
        
            stoveOnGameObject.SetActive(showVisual);
            particlesGameObject.SetActive(showVisual);
        }
    }
}
