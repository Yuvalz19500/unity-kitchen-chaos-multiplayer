using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeliveryResultUI : MonoBehaviour
    {
        private static readonly int PopupAnimId = Animator.StringToHash("Popup");

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Color successColor;
        [SerializeField] private Color failColor;
        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failSprite;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            DeliveryManager.Instance.OnOrderDelivered += DeliveryManagerOnOrderDelivered;
            DeliveryManager.Instance.OnOrderFailed += DeliveryManagerOnOrderFailed;

            gameObject.SetActive(false);
        }

        private void DeliveryManagerOnOrderFailed(object sender, EventArgs e)
        {
            backgroundImage.color = failColor;
            iconImage.sprite = failSprite;
            messageText.text = "Delivery\nFailed!";

            gameObject.SetActive(true);
            _animator.SetTrigger(PopupAnimId);
        }

        private void DeliveryManagerOnOrderDelivered(object sender, EventArgs e)
        {
            backgroundImage.color = successColor;
            iconImage.sprite = successSprite;
            messageText.text = "Delivery\nSuccess!";

            gameObject.SetActive(true);
            _animator.SetTrigger(PopupAnimId);
        }
    }
}