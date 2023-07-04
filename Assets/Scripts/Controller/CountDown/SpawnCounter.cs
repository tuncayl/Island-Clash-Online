using System;
using FishNet.Object;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers.Room
{
    public class SpawnCounter : SyncCounter, ISetCounter
    {
        #region SelfVariables


        #endregion


        #region UnityMethods

        protected override void Update()
        {
            base.Update();
            if(IsOwner is false)return;

            UiMainSignals.Instance.OnChangeSpawnCoutDown.Invoke(minute);
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnCompletedSendServer() {
            Debug.Log("ONCOMPLETED CLIENT");

            Despawn();
        }
        #endregion


        #region MainMethods

        public override void OnStartClient()
        {
            base.OnStartClient();
            if(IsOwner is false)return;

            UiMainSignals.Instance.OnActiveSpawnCoutDown.Invoke(true);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if(IsOwner is false)return;

            UiMainSignals.Instance.OnActiveSpawnCoutDown.Invoke(false);
     
        }

        protected override void OnCompletedOwner()
        {
            Debug.Log("ON COMP CLIENT SPAWN COUNT");
            Player _player = PlayerSignal.Instance.OnGetPlayer.Invoke();
            Debug.Log(_player.ResuableGeneralData.Chibicolor+"ON COMPLETE PLAYER");
            IslandSignals.Instance.OnSpawnPosition.Invoke(_player);
            UiMainSignals.Instance.OnActiveSpawnCoutDown.Invoke(false);
            OnCompletedSendServer();

            
        }

        
        protected override void OnCompleteServer()
        {
        }

        #endregion


        public void SetCounter(int timer, params object[] datas)
        {
            
            Remaning = timer;
        }
    }
}