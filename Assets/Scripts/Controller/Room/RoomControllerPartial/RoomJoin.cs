using System.Collections.Generic;
using COMMANDS.Playercommands;
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
        //JoinRoom-----------------------
        [ServerRpc(RequireOwnership = false)]
        private void JoinRoom(NetworkConnection whojoined, int roomid)
        {
            if (Checkjoin(roomid, out string failedtxt) is false)
            {
                FailedJoinRoom(whojoined, failedtxt);
                return;
            }

            MatchCondition.AddToMatch(roomid, whojoined, NetworkManager);
            UserSignals.Instance.OnSetMatchid.Invoke(whojoined,roomid);
            Rooms[roomid].Players.Add(whojoined);
            JoinedRoom(whojoined, roomid);
            for (int i = 0; i < Rooms[roomid].Players.Count; i++)
            {
                TargetRefleshRoom(Rooms[roomid].Players[i]);
            }
        }

        [TargetRpc]
        private void TargetRefleshRoom(NetworkConnection con) => RefleshRoom();


        [TargetRpc]
        private void JoinedRoom(NetworkConnection con, int id)
        {
            currentRoomid = id;
            PanelSignals.Instance.OnMenuChange.Invoke(menus.create);
            RoomSignals.Instance.OnCreatedRoom.Invoke(id);
        }

        [TargetRpc]
        private void FailedJoinRoom(NetworkConnection con, string failedtxt)
        {
            info.Infotext = failedtxt;
            PopupManager.Instance.Show("info");
        }
           


        private bool Checkjoin(int roomid, out string failedtxt)
        {
            failedtxt = "";
            if (Rooms.ContainsKey(roomid) is false)
            {
                failedtxt = "Room is not found";
                return false;
            }
            if (Rooms[roomid].Players.Count + 1 > Rooms[roomid].Capacity)
            {
                failedtxt = "Room is full players";
                return false;
            }

            if (Rooms[roomid].isrunnig)
            {
                failedtxt = "Room is runnig";
                return false;
            }

            return true;
        }
    }
}