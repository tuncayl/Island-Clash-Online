using FishNet.Connection;
using FishNet.Object;
using onlinetutorial.controllers;
using UnityEngine;

namespace onlinetutorial.interfaces
{
    public interface IPlayer
    {
        public void SetPlayer(PlayerSetArgs args);
        public Player GetPlayer();
        
    }

    public struct PlayerSetArgs
    {
        public PlayerSetArgs(Vector3 _startpositon, int _order,int _roomid)
        {
            Startpositon = _startpositon;
            Order = _order;
            roomid = _roomid;
        }

        public Vector3 Startpositon;
        public int Order;
        public int roomid;
    }
}