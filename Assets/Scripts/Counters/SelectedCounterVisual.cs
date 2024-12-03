using UnityEngine;

namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter baseCounter;
        [SerializeField] private GameObject[] visualGameObjects;

        private void Start()
        {
            Player.Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
        }

        private void PlayerOnSelectedCounterChanged(object sender, Player.Player.OnSelectedCounterChangeEventArgs e)
        {
            foreach (GameObject go in visualGameObjects) go.SetActive(e.SelectedCounter == baseCounter);
        }
    }
}