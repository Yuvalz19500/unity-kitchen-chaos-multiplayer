using System;
using Counters;
using Sound;
using UI;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private float warningSoundTimer = .2f;

    private AudioSource _audioSource;
    private float _warningSoundTimer;
    private bool _playWarningSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStoveStateChanged += StoveCounterOnStoveStateChanged;
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
    }

    private void Update()
    {
        if (!_playWarningSound) return;

        _warningSoundTimer -= Time.deltaTime;
        if (!(_warningSoundTimer <= 0)) return;

        _warningSoundTimer = warningSoundTimer;
        SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e)
    {
        _playWarningSound = stoveCounter.IsFried() && e.ProgressNormalized >= stoveCounter.GetWarningThreshold();
    }

    private void StoveCounterOnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedArgs e)
    {
        if (e.NewStoveCounterState is StoveCounter.StoveCounterState.Frying or StoveCounter.StoveCounterState.Fried)
            _audioSource.Play();
        else
            _audioSource.Pause();
    }
}