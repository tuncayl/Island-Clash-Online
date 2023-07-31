

using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using onlinetutorial.interfaces;
using onlinetutorial.controllers;
using onlinetutorial.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace COMMANDS.Playercommands
{
    public class LoadMainScene:SceneCommand
    {

        private NetworkManager NetworkManager;

        private List<NetworkConnection> Players;

        public LoadMainScene(NetworkManager _networkManager,ref List<NetworkConnection> _players)
        {
            NetworkManager = _networkManager;
            Players = _players;
        }
        
        
        public override void execute()
        {
            SceneLoadData sld = new SceneLoadData("main");
            sld.ReplaceScenes = ReplaceOption.None;
            sld.Options.AutomaticallyUnload = true;
            sld.Options.AllowStacking = true;
            sld.Options.LocalPhysics = LocalPhysicsMode.Physics3D;
            sld.MovedNetworkObjects = this.CoppyNetworkObjects(ref Players);
            NetworkConnection[] conns = new NetworkConnection[Players.Count];
            Players.CopyTo(conns);
            NetworkManager.SceneManager.LoadConnectionScenes(conns, sld);
        }
    }
}