using System;
using Counters;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStoveStateChanged += StoveCounterOnStoveStateChanged;
    }

    private void StoveCounterOnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedArgs e)
    {
        if (e.NewStoveCounterState is StoveCounter.StoveCounterState.Frying or StoveCounter.StoveCounterState.Fried)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }
}
