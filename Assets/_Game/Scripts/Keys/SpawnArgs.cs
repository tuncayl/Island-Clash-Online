using FishNet.Connection;
using UnityEngine;

namespace Keys
{
    public struct SpawnArgs
    {
        public SpawnArgs(Transform _parent,NetworkConnection _connection,int _roomid)
        {
            Connection = _connection;
            Parent = _parent;
            roomid = _roomid;
        }

        public Transform Parent;
        public NetworkConnection Connection;
        public int roomid;

    }
}