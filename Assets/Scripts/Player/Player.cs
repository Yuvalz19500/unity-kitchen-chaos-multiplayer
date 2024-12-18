using System;
using System.Collections.Generic;
using Counters;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        public static Player LocalInstance { get; private set; }

        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private LayerMask countersLayerMask;
        [SerializeField] private LayerMask collisionsLayerMask;
        [SerializeField] private GameObject kitchenObjectHoldPoint;
        [SerializeField] private List<Vector3> spawnPoints;

        private bool _isWalking;
        private KitchenObject _kitchenObject;
        private Vector3 _lastInteractDirection;
        private BaseCounter _selectedCounter;

        public static event EventHandler OnAnyPlayerSpawned;
        public static event EventHandler OnAnyPickedSomething;
        public event EventHandler OnPickSomething;
        public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;

        public class OnSelectedCounterChangeEventArgs : EventArgs
        {
            public BaseCounter SelectedCounter;
        }

        public static void ResetStaticData()
        {
            OnAnyPlayerSpawned = null;
            OnAnyPickedSomething = null;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner) LocalInstance = this;

            transform.position = spawnPoints[(int)OwnerClientId];
            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

            if (IsServer)
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisconnectCallback;
        }

        private void NetworkManagerOnClientDisconnectCallback(ulong clientId)
        {
            if (OwnerClientId == clientId && HasKitchenObject())
                KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
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

            if (!_kitchenObject) return;

            OnPickSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
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

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
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
            Vector3 moveDirection = new(inputVector.x, 0, inputVector.y);

            float moveDistance = moveSpeed * Time.deltaTime;
            const float playerRadius = 0.7f;
            const float playerHeight = 2f;
            bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius,
                moveDirection, Quaternion.identity, moveDistance, collisionsLayerMask);

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

            canMove = moveDirection.x is < -.5f or > .5f && !Physics.BoxCast(transform.position,
                Vector3.one * playerRadius,
                moveDirectionX, Quaternion.identity, moveDistance, collisionsLayerMask);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new(0, 0, moveDirection.z);
                moveDirectionZ.Normalize();

                canMove = moveDirection.z is < -.5f or > .5f && !Physics.BoxCast(transform.position,
                    Vector3.one * playerRadius,
                    moveDirectionZ, Quaternion.identity, moveDistance, collisionsLayerMask);

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