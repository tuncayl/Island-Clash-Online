using System.Collections.Generic;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using Keys;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public partial class RoomController : NetworkBehaviour
    {
        //RoomList-----------------------
        [ServerRpc(RequireOwnership = false)]
        private void ListRooms (NetworkConnection who)
        {
            List<RoomData> Roomsinfo = new List<RoomData>();
            if (Rooms.Count == 0)
            {
                ListedRooms(who,Roomsinfo);
                return;
            }
            foreach (KeyValuePair<int,room> pair in Rooms)
            {
                Roomsinfo.Add(new RoomData(pair.Value.Id,pair.Value.Players.Count));
            }
            ListedRooms(who,Roomsinfo);
        }

        

        [TargetRpc]
        private void ListedRooms (NetworkConnection con,List<RoomData> rooms)
        {
            UiMenuSignals.Instance.OnUiListedRoom.Invoke(ref rooms);
        }
    }
}