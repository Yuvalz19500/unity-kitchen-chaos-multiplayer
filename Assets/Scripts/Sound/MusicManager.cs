using System;
using UnityEngine;

namespace Sound
{
    public class MusicManager : MonoBehaviour
    {
        private const string PlayerPrefsMusicVolume = "MusicVolume";

        public static MusicManager Instance { get; private set; }

        private float _volume = .3f;
        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            _audioSource = GetComponent<AudioSource>();

            _volume = PlayerPrefs.GetFloat(PlayerPrefsMusicVolume, 1f);
            _audioSource.volume = _volume;
        }

        public void ChangeVolume()
        {
            _volume += .1f;

            if (_volume > 1f) _volume = 0f;

            _audioSource.volume = _volume;

            PlayerPrefs.SetFloat(PlayerPrefsMusicVolume, _volume);
            PlayerPrefs.Save();
        }

        public float GetVolume()
        {
            return _volume;
        }
    }
}