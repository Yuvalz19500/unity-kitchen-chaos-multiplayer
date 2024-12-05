using Sound;
using UnityEngine;

namespace Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private float footstepTimerMax = 0.1f;

        private float _footstepTimer;

        private void Update()
        {
            // if (!Player.Instance.IsWalking()) return;
            //
            // _footstepTimer -= Time.deltaTime;
            //
            // if (!(_footstepTimer <= 0)) return;
            //
            // _footstepTimer = footstepTimerMax;
            // SoundManager.Instance.PlayFootstepSound(Player.Instance.transform.position);
        }
    }
}