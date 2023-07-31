using System.Collections.Generic;
using COMMANDS.Playercommands;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public partial class RoomController : NetworkBehaviour
    {

        
        [ServerRpc(RequireOwnership = false)]
        private void StartCount(int roomid)
        {
            foreach (NetworkConnection con in Rooms[roomid].Players)OnStartRoomCounter(con);
            new LoadRoomScene(NetworkManager,ref Rooms[roomid].Players, roomid).execute();;
            Rooms[roomid].isrunnig = true;
      
        }

        
        
        [TargetRpc]
        private void OnStartRoomCounter(NetworkConnection con) => RoomSignals.Instance.OnSetActiveRoomCounter.Invoke(true);


    }
}