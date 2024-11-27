using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask countersLayerMask;

    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private ClearCounter _selectedCounter;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null) _selectedCounter.Interact();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        const float playerRadius = 0.7f;
        const float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirection, moveDistance);

        canMove = HandleWallHugging(canMove, playerHeight, playerRadius, moveDistance, ref moveDirection);

        if (canMove) transform.position += moveDirection * moveDistance;

        _isWalking = moveDirection != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }

    private bool HandleWallHugging(bool canMove, float playerHeight, float playerRadius, float moveDistance,
        ref Vector3 moveDirection)
    {
        if (canMove) return true;

        Vector3 moveDirectionX = new(moveDirection.x, 0, 0);
        moveDirectionX.Normalize();

        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirectionX, moveDistance);

        if (canMove)
        {
            moveDirection = moveDirectionX;
        }
        else
        {
            Vector3 moveDirectionZ = new(0, 0, moveDirection.z);
            moveDirectionZ.Normalize();

            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirectionZ, moveDistance);

            if (canMove) moveDirection = moveDirectionZ;
        }

        return canMove;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, 0, inputVector.y);

        if (moveDirection != Vector3.zero) _lastInteractDirection = moveDirection;

        if (Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance,
                countersLayerMask))
        {
            if (!raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) return;

            if (clearCounter != _selectedCounter)
                SetSelectedCounter(clearCounter);
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        _selectedCounter = clearCounter;

        OnSelectedCounterChanged?.Invoke(this,
            new OnSelectedCounterChangeEventArgs { SelectedCounter = _selectedCounter });
    }

    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public ClearCounter SelectedCounter;
    }
}