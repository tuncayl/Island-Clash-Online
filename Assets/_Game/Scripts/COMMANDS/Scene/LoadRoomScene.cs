

using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using onlinetutorial.interfaces;
using onlinetutorial.controllers;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COMMANDS.Playercommands
{
    public class LoadRoomScene:SceneCommand
    {
        public NetworkManager NetworkManager;
        public List<NetworkConnection> Players;
        public int Roomid;
        public LoadRoomScene(NetworkManager _networkManager,ref List<NetworkConnection> _players,int _roomid)
        {
            NetworkManager = _networkManager;
            Players = _players;
            Roomid = _roomid;
        }
        
        
        public override void execute()
        {
            SceneLoadData sld = new SceneLoadData("room");
            sld.ReplaceScenes = ReplaceOption.None;
            sld.Options.AutomaticallyUnload = true;
            sld.Options.AllowStacking = true;
            NetworkConnection[] conns = new NetworkConnection[Players.Count];
            Players.CopyTo(conns);
            NetworkManager.SceneManager.LoadConnectionScenes(conns, sld);
            SceneSignals.Instance.OnRoomSceneload.Invoke(ref Roomid);

        }
    }
}