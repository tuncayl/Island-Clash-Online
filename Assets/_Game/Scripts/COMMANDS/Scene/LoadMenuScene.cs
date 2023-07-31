using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using onlinetutorial.Extensions;
using onlinetutorial.Signals;

namespace COMMANDS.Playercommands
{
    public class LoadMenuScene : SceneCommand
    {
        public NetworkManager NetworkManager;
        public NetworkConnection Player;

        public LoadMenuScene(NetworkManager _networkManager, NetworkConnection _player)
        {
            NetworkManager = _networkManager;
            Player = _player;
        }


        public override void execute()
        {
            SceneLoadData sld = new SceneLoadData("menu");
            sld.ReplaceScenes = ReplaceOption.OnlineOnly;
            sld.MovedNetworkObjects = new NetworkObject[] { Player.FirstObject };
            NetworkConnection[] conns = new[] { Player };
            NetworkManager.SceneManager.LoadConnectionScenes(conns, sld);
        }
    }
}