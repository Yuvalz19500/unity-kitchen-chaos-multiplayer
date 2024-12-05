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

        private void Update()
        {
            if (!IsOwner) return;

            _animator.SetBool(IsWalkingAnimId, player.IsWalking());
        }
    }
}