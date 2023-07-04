using System;
using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class RoomSignals : MonoSingleton<RoomSignals>
    {
        public UnityAction<int> OnCreatedRoom = delegate(int arg0) { };

        public UnityAction<int> OnJoinRoom = delegate(int arg0) { };

        public UnityAction<bool> OnChangeHost = delegate(bool arg0) { };

        public UnityAction<int> OnClearRoom = delegate(int arg0) { };
        public Func<int> OnGetRoomid = delegate { return default; };

        public Func<int, List<NetworkConnection>> OnGetRoomConnections = delegate(int i) { return default; };

        public UnityAction<bool> OnSetActiveRoomCounter = delegate { };
        public UnityAction<int> OnStartRoom = delegate(int arg1) { };
    }
}