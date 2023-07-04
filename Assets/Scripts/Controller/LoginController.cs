using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace onlinetutorial.controllers
{
    public class LoginController : MonoBehaviour
    {
        #region SelfVariables

        [SerializeField] private bool ServerStart = false;

        #endregion

        #region UnityMethods

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            UnSubscire();
        }

        private void Start()
        {
            Subscire();

            if (InstanceFinder.IsOffline is false) return;
            
            if (ServerStart is true) InstanceFinder.ServerManager.StartConnection();
            else DelayStart();
        }

        #endregion

        #region OtherMehods

        private void OnClientConnection(ClientConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState != LocalConnectionState.Started) return;
            Debug.Log("CLİENTCONNECTİON");
            CoreGameSignals.Instance.OnConnectedClient.Invoke();
            
        }
        private void OnServerConnection(ServerConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState != LocalConnectionState.Started) return;
            Debug.Log("SERVERCONNECTİON");
            CoreGameSignals.Instance.OnConnectedServer.Invoke();
            

        }

        private async UniTask DelayStart()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            InstanceFinder.ClientManager.StartConnection();
        }

        #endregion


        #region SubscireMethods

        private void Subscire()
        {
            InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnection;
            InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnection;
        }

        private void UnSubscire()
        {
            if(InstanceFinder.ClientManager is null) return;
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnection;
            InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnection;

        }

        #endregion
    }
}