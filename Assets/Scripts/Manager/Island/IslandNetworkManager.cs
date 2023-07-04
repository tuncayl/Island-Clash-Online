using FishNet.Object;
using FishNet.Object.Synchronizing;
using Keys;
using onlinetutorial.controllers.Island;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class IslandNetworkManager : NetworkBehaviour, IIsland
{
    #region SelfVariables

    [SyncVar] private int IslandRoomıd;


    private IslandSpawnController _IslandSpawnController;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        _IslandSpawnController = GetComponent<IslandSpawnController>();
    }


    private void OnDisable()
    {
        UnSubscire();
    }

    #endregion

    #region ServerMethods

    [ObserversRpc(BufferLast = true)]
    private void StartSpawnChibi()
    {
        _IslandSpawnController.SpawnChibi(UserSignals.Instance.OnGetUserspawnargs.Invoke());
    }
    

    #endregion
    #region OtherMethods

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (IsServer)
        {
            _IslandSpawnController.INIT();
            this.SetIsland(SceneSignals.Instance.OnGetMainSceneInfo.Invoke());
            StartSpawnChibi();
            _IslandSpawnController.SpawnGameCounter(120,IslandRoomıd);
        }

        if (IsClient)
        {
            Subscire();
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    
    private int OnGetIslandId() => IslandRoomıd;

    #endregion


    #region SubscireMethods

    private void Subscire()
    {
        if (IsServer) return;
        IslandSignals.Instance.OnGetIslandId += OnGetIslandId;
    }

    private void UnSubscire()
    {
        if (IsServer) return;
        IslandSignals.Instance.OnGetIslandId -= OnGetIslandId;
    }

    #endregion

    public void SetIsland(Sceneinfo sceneinfo)
    {
        IslandRoomıd = sceneinfo.RoomData.Id;
    }
}