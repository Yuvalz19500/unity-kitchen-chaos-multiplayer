using System;
using UnityEngine;


namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter baseCounter;
        [SerializeField] private GameObject[] visualGameObjects;

        private void Start()
        {
            if (Player.Player.LocalInstance != null)
                Player.Player.LocalInstance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
            else
                Player.Player.OnAnyPlayerSpawned += PlayerOnAnyPlayerSpawned;
        }

        private void PlayerOnAnyPlayerSpawned(object sender, EventArgs e)
        {
            if (Player.Player.LocalInstance == null) return;

            Player.Player.LocalInstance.OnSelectedCounterChanged -= PlayerOnSelectedCounterChanged;
            Player.Player.LocalInstance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
        }

        private void PlayerOnSelectedCounterChanged(object sender, Player.Player.OnSelectedCounterChangeEventArgs e)
        {
            foreach (GameObject go in visualGameObjects) go.SetActive(e.SelectedCounter == baseCounter);
        }
    }
}