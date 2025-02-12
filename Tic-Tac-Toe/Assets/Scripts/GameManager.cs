using System;
using Unity.Multiplayer.Playmode;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    

    public event EventHandler<onCickedOnGridPositionEventArgs> onClickedonGridPosition;
    public event EventHandler onGameStarted;
    public class onCickedOnGridPositionEventArgs: EventArgs
    {
        public int x;
        public int y;
        public playerType PlayerType;
    }

    public enum playerType
    {
        None,
        Cross,
        Circle,

    }

    private playerType localPlayer;
    private NetworkVariable<playerType> currentPlayer = new NetworkVariable<playerType>();

 
    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {

        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            localPlayer = playerType.Cross;
        }
        else
        {
            localPlayer = playerType.Circle;
        }

        if (IsServer)
        {
            currentPlayer.Value = playerType.Cross;

            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }
        Debug.Log(NetworkManager.Singleton.LocalClientId + " " + localPlayer);
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if(NetworkManager.Singleton.ConnectedClients.Count >= 2)
        {
            //Start the game
            onGameStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Rpc(SendTo.Server)]
    public void positionClickedRpc(int posX, int posY, playerType PlayerType)
    {
        if (PlayerType != currentPlayer.Value)
        {
            return;
        }
        Debug.Log("Clicked" + posX + posY);
        onClickedonGridPosition?.Invoke(this, new onCickedOnGridPositionEventArgs
        {
            x = posX,
            y = posY,
            PlayerType = PlayerType,
        }) ;

        switch (currentPlayer.Value)
        {
            default:
            case playerType.Cross:
                currentPlayer.Value = playerType.Circle;
                break;
            case playerType.Circle:
                currentPlayer.Value = playerType.Cross;
                break;
        }

    }

    public playerType GetLocalPlayerType()
    {
        return localPlayer;
    }

    public playerType getCurrentPlayer()
    {
        return currentPlayer.Value;
    }
}
