using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers.Room
{
    public class GameCounter : SyncCounter, ISetCounter
    {
        #region SelfVariables

        private int? Roomid;

        #endregion


        #region UnityMethods

        protected override void Update()
        {
            base.Update();
            if (IsServer) return;
            UiMainSignals.Instance.OnChangeGameCoutDown.Invoke(minute);
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnCompletedSendServer()
        {
            Debug.Log("ONCOMPLETED CLIENT");
        }

        #endregion


        #region MainMethods

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner is false) return;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (IsOwner is false) return;
        }

        protected override void OnCompletedOwner()
        {
            OnCompletedSendServer();
        }


        protected override void OnCompleteServer()
        {
            List<NetworkConnection> networkConnections = RoomSignals.Instance.OnGetRoomConnections.Invoke(Roomid.Value);
            for (int i = 0; i < networkConnections.Count; i++)
            {
                if (networkConnections[i].FirstObject.transform.GetChild(0)
                    .TryGetComponent<IPlayer>(out IPlayer _player))
                {
                    _player.GetPlayer()._PlayerLocalSignals.OnGameEnd.Invoke();
                }
            }
            RoomSignals.Instance.OnClearRoom.Invoke(Roomid.Value);
        }

        #endregion


        public void SetCounter(int timer, params object[] datas)
        {
            Roomid = datas[0] as int?;
            Remaning = timer;
        }
    }
}