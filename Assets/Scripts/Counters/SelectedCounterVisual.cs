using UnityEngine;

namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter baseCounter;
        [SerializeField] private GameObject[] visualGameObjects;

        private void Start()
        {
            Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
        }

        private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangeEventArgs e)
        {
            foreach (GameObject go in visualGameObjects) go.SetActive(e.SelectedCounter == baseCounter);
        }
    }
}