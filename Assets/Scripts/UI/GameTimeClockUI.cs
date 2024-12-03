using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameTimeClockUI : MonoBehaviour
    {
        [SerializeField] private Image clockImage;

        private void Update()
        {
            clockImage.fillAmount = GameManager.Instance.GetPlayingTimerNormalized();
        }
    }
}