using System.Collections.Generic;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public partial class RoomController : NetworkBehaviour
    {

        //ExitRoom-----------------------
        [ServerRpc(RequireOwnership = false)]
        private void ExitRoom(NetworkConnection whocreated, int roomid)
        {
            MatchCondition.RemoveFromMatch(roomid, whocreated, NetworkManager);
            Rooms[roomid].Players.Remove(whocreated);
            
            ServerChangeHost(whocreated,false);
            UserSignals.Instance.OnSetMatchid.Invoke(whocreated,-1);
            if (Rooms[roomid].Players.Count == 0)
            {
                Rooms.Remove(roomid);
                return;
            }
            
            for (int i = 0; i < Rooms[roomid].Players.Count; i++) 
                TargetRefleshRoom(Rooms[roomid].Players[i]);
            
            ServerChangeHost(Rooms[roomid].Players[0],true,roomid);

        }
        [Server]
        private void DisconnectedRoom(NetworkConnection whocreated, int roomid)
        {
            MatchCondition.RemoveFromMatch(roomid, whocreated, NetworkManager);
            Rooms[roomid].Players.Remove(whocreated);

            Debug.Log(Rooms[roomid].Players.Count);
            if (Rooms[roomid].Players.Count == 0)
            {
                Rooms.Remove(roomid);
                
            }
            
        }

        [Server]
        private void ServerChangeHost(NetworkConnection con, bool host, int? roomid = null)
        {
            if (roomid.HasValue) Rooms[roomid.Value].Host = con;
            TargetChangeHost(con, host);
        }

        [TargetRpc]
        private void TargetChangeHost(NetworkConnection con, bool host)
        {
            Host = host;
            RoomSignals.Instance.OnChangeHost.Invoke(host);
            if(!Host) PanelSignals.Instance.OnMenuChange.Invoke(menus.roomback);
            
        }


        [Server]
        private void OnClearRoom(int roomid)
        {
            if (Rooms.ContainsKey(roomid) is false) return;
            Rooms.Remove(roomid);
        }

    }
}