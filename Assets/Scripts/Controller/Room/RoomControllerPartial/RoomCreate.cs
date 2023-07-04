using System.Collections.Generic;
using COMMANDS.Playercommands;
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
        //Create Room--------------------
        [ServerRpc(RequireOwnership = false)]
        private void CreateRoom(NetworkConnection whocreated)
        {
            room newrom = new room
                { Id = roomIdCounterProperty, Players = new List<NetworkConnection>() { whocreated } };
            MatchCondition.AddToMatch(roomIdCounterProperty, whocreated, NetworkManager);
            UserSignals.Instance.OnSetMatchid.Invoke(whocreated, newrom.Id);
            Rooms.Add(roomIdCounterProperty, newrom);
            CreatedRoom(whocreated, newrom.Id);
            ServerChangeHost(whocreated,true,roomIdCounterProperty);
            Debug.Log(whocreated + " crteated"+"NEW ROOM ID"+newrom.Id);

            ++roomIdCounterProperty;
        }

        [TargetRpc]
        private void CreatedRoom(NetworkConnection con, int id)
        {
            currentRoomid = id;
            PanelSignals.Instance.OnMenuChange.Invoke(menus.create);
            RoomSignals.Instance.OnCreatedRoom.Invoke(id);
            RefleshRoom();
        }
    }
}