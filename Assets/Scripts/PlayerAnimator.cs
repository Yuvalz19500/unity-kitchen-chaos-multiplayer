using UnityEngine;

public class PlayerAnimator : MonoBehaviour
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
        _animator.SetBool(IsWalkingAnimId, player.IsWalking());
    }
}