using System;
using Sound;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountdownUI : MonoBehaviour
    {
        private static readonly int NumberPopupAnimId = Animator.StringToHash("NumberPopup");

        [SerializeField] private TextMeshProUGUI countdownText;

        private Animator _animator;
        private int _previousCountdownNumber;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!GameManager.Instance.IsCountdownToStartActive()) return;

            int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownTimer());
            countdownText.text = countdownNumber.ToString();

            if (countdownNumber == _previousCountdownNumber) return;

            _animator.SetTrigger(NumberPopupAnimId);
            SoundManager.Instance.PlayCountdownSound();
            _previousCountdownNumber = countdownNumber;
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            gameObject.SetActive(GameManager.Instance.IsCountdownToStartActive());
        }
    }
}