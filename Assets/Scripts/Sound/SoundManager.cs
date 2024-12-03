using System;
using Counters;
using ScriptableObjects;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        private const string PlayerPrefsSoundVolume = "SoundEffectsVolume";

        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioClipRefsSO audioClipRefsSO;

        private float _volume = .9f;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            _volume = PlayerPrefs.GetFloat(PlayerPrefsSoundVolume, 1f);
        }

        private void Start()
        {
            DeliveryManager.Instance.OnOrderDelivered += DeliveryManagerOnOrderDelivered;
            DeliveryManager.Instance.OnOrderFailed += DeliveryManagerOnOrderFailed;
            CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;
            Player.Player.Instance.OnPickSomething += PlayerOnPickSomething;
            BaseCounter.OnAnyObjectPlaced += BaseCounterOnAnyObjectPlaced;
            TrashCounter.OnAnyObjectTrashed += TrashCounterOnAnyObjectTrashed;
        }

        private void TrashCounterOnAnyObjectTrashed(object sender, EventArgs e)
        {
            TrashCounter thrashCounter = sender as TrashCounter;
            Debug.Assert(thrashCounter != null, nameof(thrashCounter) + " != null");
            PlaySound(audioClipRefsSO.trash, thrashCounter.transform.position);
        }

        private void BaseCounterOnAnyObjectPlaced(object sender, EventArgs e)
        {
            BaseCounter baseCounter = sender as BaseCounter;
            Debug.Assert(baseCounter != null, nameof(baseCounter) + " != null");
            PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
        }

        private void PlayerOnPickSomething(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.objectPickup, Player.Player.Instance.transform.position);
        }

        private void CuttingCounterOnAnyCut(object sender, EventArgs e)
        {
            CuttingCounter cuttingCounter = sender as CuttingCounter;
            Debug.Assert(cuttingCounter != null, nameof(cuttingCounter) + " != null");
            PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
        }

        private void DeliveryManagerOnOrderFailed(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
        }

        private void DeliveryManagerOnOrderDelivered(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
        }

        private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, _volume * volumeMultiplier);
        }

        private void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultiplier = 1f)
        {
            PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, _volume * volumeMultiplier);
        }

        public void PlayFootstepSound(Vector3 position)
        {
            PlaySound(audioClipRefsSO.footstep, position);
        }

        public void ChangeVolume()
        {
            _volume += .1f;

            if (_volume > 1f) _volume = 0f;

            PlayerPrefs.SetFloat(PlayerPrefsSoundVolume, _volume);
            PlayerPrefs.Save();
        }

        public float GetVolume()
        {
            return _volume;
        }
    }
}