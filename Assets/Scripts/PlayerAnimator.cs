using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsWalkingAnimId = Animator.StringToHash("IsWalking");

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IsWalkingAnimId, player.IsWalking());
    }
}