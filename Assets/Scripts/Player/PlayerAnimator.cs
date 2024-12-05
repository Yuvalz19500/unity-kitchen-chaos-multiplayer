using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : NetworkBehaviour
    {
        private static readonly int IsWalkingAnimId = Animator.StringToHash("IsWalking");

        [SerializeField] private Player player;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        [ServerRpc]
        private void PlayWalingAnimationServerRPC()
        {
            _animator.SetBool(IsWalkingAnimId, player.IsWalking());
        }

        private void Update()
        {
            if (!IsOwner) return;

            PlayWalingAnimationServerRPC();
        }
    }
}