using System;
using Counters;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private LayerMask countersLayerMask;
        [SerializeField] private GameObject kitchenObjectHoldPoint;

        private bool _isWalking;
        private KitchenObject _kitchenObject;
        private Vector3 _lastInteractDirection;
        private BaseCounter _selectedCounter;

        public event EventHandler OnPickSomething;
        public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;

        public class OnSelectedCounterChangeEventArgs : EventArgs
        {
            public BaseCounter SelectedCounter;
        }

        private void Start()
        {
            GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
            GameInput.Instance.OnInteractAlternateAction += GameInputOnInteractAlternateAction;
        }

        private void Update()
        {
            if (!IsOwner) return;

            HandleMovement();
            HandleInteractions();
        }

        public Transform GetKitchenObjectFollowTransform()
        {
            return kitchenObjectHoldPoint.transform;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            _kitchenObject = kitchenObject;

            if (_kitchenObject) OnPickSomething?.Invoke(this, EventArgs.Empty);
        }

        public KitchenObject GetKitchenObject()
        {
            return _kitchenObject;
        }

        public void ClearKitchenObject()
        {
            _kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return _kitchenObject != null;
        }


        private void GameInputOnInteractAction(object sender, EventArgs e)
        {
            if (!GameManager.Instance.IsGamePlaying()) return;

            if (_selectedCounter != null) _selectedCounter.Interact(this);
        }

        private void GameInputOnInteractAlternateAction(object sender, EventArgs e)
        {
            if (!GameManager.Instance.IsGamePlaying()) return;

            if (_selectedCounter != null) _selectedCounter.InteractAlternate(this);
        }

        public bool IsWalking()
        {
            return _isWalking;
        }

        private void HandleMovement()
        {
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            HandleMovementServerRPC(inputVector);
        }

        [ServerRpc]
        private void HandleMovementServerRPC(Vector2 inputVector)
        {
            if (!IsServer) return;

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

            canMove = moveDirection.x is < -.5f or > .5f && !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new(0, 0, moveDirection.z);
                moveDirectionZ.Normalize();

                canMove = moveDirection.z is < -.5f or > .5f && !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius, moveDirectionZ, moveDistance);

                if (canMove) moveDirection = moveDirectionZ;
            }

            return canMove;
        }

        private void HandleInteractions()
        {
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            Vector3 moveDirection = new(inputVector.x, 0, inputVector.y);

            if (moveDirection != Vector3.zero) _lastInteractDirection = moveDirection;

            if (Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance,
                    countersLayerMask))
            {
                if (!raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) return;

                if (baseCounter != _selectedCounter)
                    SetSelectedCounter(baseCounter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }

        private void SetSelectedCounter(BaseCounter baseCounter)
        {
            _selectedCounter = baseCounter;

            OnSelectedCounterChanged?.Invoke(this,
                new OnSelectedCounterChangeEventArgs { SelectedCounter = _selectedCounter });
        }
    }
}