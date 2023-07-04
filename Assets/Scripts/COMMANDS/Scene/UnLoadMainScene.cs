using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using onlinetutorial.Extensions;
using onlinetutorial.Signals;

namespace COMMANDS.Playercommands
{
    public class UnLoadMainScene : SceneCommand
    {
        public NetworkManager NetworkManager;
        public NetworkConnection Player;
        public int Roomid;

        public UnLoadMainScene(NetworkManager _networkManager, ref NetworkConnection _player, int _roomid)
        {
            NetworkManager = _networkManager;
            Player = _player;
            Roomid = _roomid;
        }


        public override void execute()
        {
            SceneUnloadData sld = new SceneUnloadData("main");
            NetworkManager.SceneManager.UnloadConnectionScenes(Player, sld);
        }
    }
}