using System;
using SceneManagement;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectListSO kitchenObjectsListSO;
    [SerializeField] private int maxPlayers = 4;

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex,
        NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        GameObject kitchenGameObject = Instantiate(kitchenObjectSO.prefab);
        kitchenGameObject.GetComponent<NetworkObject>().Spawn(true);

        KitchenObject kitchenObject = kitchenGameObject.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent =
            kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }

    public int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenObjectsListSO.kitchenObjectsList.IndexOf(kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSOFromIndex(int index)
    {
        return kitchenObjectsListSO.kitchenObjectsList[index];
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);

        ClearKitchenObjectOnParentClientRpc(kitchenObjectNetworkObjectReference);

        kitchenObjectNetworkObject.Despawn();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectParent();
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,
        IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(Instance.GetKitchenObjectSOIndex(kitchenObjectSO),
            kitchenObjectParent.GetNetworkObject());
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManagerConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManagerConnectionApprovalCallback(
        NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
        NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scenes.CharacterSelectScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= maxPlayers)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisconnectCallback;

        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManagerOnClientDisconnectCallback(ulong obj)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }
}