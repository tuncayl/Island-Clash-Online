using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using onlinetutorial.Extensions;
using onlinetutorial.Signals;

namespace COMMANDS.Playercommands
{
    public class UnLoadRoomScene : SceneCommand
    {
        public NetworkManager NetworkManager;
        public List<NetworkConnection> Players;

        public UnLoadRoomScene(NetworkManager _networkManager, List<NetworkConnection> _players)
        {
            NetworkManager = _networkManager;
            Players = _players;
        }


        public override void execute()
        {
            SceneUnloadData sld = new SceneUnloadData("room");
            NetworkConnection[] connections = this.CoppyNetworkConnections(ref Players);
            NetworkManager.SceneManager.UnloadConnectionScenes(connections, sld);
        }
    }
}