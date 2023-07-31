using COMMANDS.Playercommands;
using FishNet.Connection;
using FishNet.Object;
using onlinetutorial.controllers;
using onlinetutorial.Signals;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Manager.Scene
{
    public class SceneManager : NetworkBehaviour
    {
        #region SelfVariables

        #endregion

        #region UnityMethods

        public override void OnStartClient()
        {
            base.OnStartClient();
            Subscire();
        }

        private void OnDisable()
        {
            UnSubscire();
        }

        #endregion

        #region OtherMethods

        #endregion

        #region ServerMethods

        private void OnMenuLoad()
        {
            SceneSignals.Instance.OnMenuSceneLoading.Invoke();
            ServerMenuLoad(PlayerSignal.Instance.OnGetPlayer.Invoke().Owner,
                IslandSignals.Instance.OnGetIslandId.Invoke());
        }

        [ServerRpc(RequireOwnership = false)]
        private void ServerMenuLoad(NetworkConnection con, int roomid) => new LoadMenuScene(NetworkManager, con).execute();

        #endregion

        #region SubscireMethods

        private void Subscire()
        {
            ButtonSignals.Instance.OnMenuLoad += OnMenuLoad;
        }


        private void UnSubscire()
        {
            ButtonSignals.Instance.OnMenuLoad -= OnMenuLoad;
        }

        #endregion
    }
}