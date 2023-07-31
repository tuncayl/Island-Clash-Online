using System.Collections.Generic;
using COMMANDS.Playercommands;

using FishNet.Object;
using Keys;
using onlinetutorial.Signals;



namespace onlinetutorial.controllers
{
    public partial class RoomController : NetworkBehaviour
    {

        [Server]
        private void StartRoom(int roomid)
        {
            new LoadMainScene(NetworkManager,ref Rooms[roomid].Players).execute();
            SceneSignals.Instance.OnMainSceneload.Invoke(
                new Sceneinfo(
                    new RoomData(roomid, Rooms[roomid].Players.Count)));
        }



    }
}