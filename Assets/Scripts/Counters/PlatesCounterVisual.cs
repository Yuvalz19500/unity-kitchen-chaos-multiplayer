using System;
using System.Collections.Generic;
using UnityEngine;

namespace Counters
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private GameObject counterTopPoint;
        [SerializeField] private GameObject plateVisualPrefab;
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private float plateVisualOffset = 0.1f;
        
        private readonly List<GameObject> _platesVisuals = new List<GameObject>();

        private void Start()
        {
            platesCounter.OnPlateSpawned += PlatesCounterOnPlateSpawned;
            platesCounter.OnPlateTaken += PlatesCounterOnPlateTaken;
        }

        private void PlatesCounterOnPlateTaken(object sender, EventArgs e)
        {
            GameObject plateToRemoveGameObject = _platesVisuals[^1];
            _platesVisuals.Remove(plateToRemoveGameObject);
            Destroy(plateToRemoveGameObject);
        }

        private void PlatesCounterOnPlateSpawned(object sender, EventArgs e)
        {
            GameObject plateVisualGameObject = Instantiate(plateVisualPrefab, counterTopPoint.transform);

            plateVisualGameObject.transform.localPosition = new Vector3(0f, _platesVisuals.Count * plateVisualOffset, 0f);
            _platesVisuals.Add(plateVisualGameObject);
        }
    }
}
