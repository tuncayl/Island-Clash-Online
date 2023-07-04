using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine.SceneManagement;

namespace Keys
{
    [System.Serializable]
    public record room
    {
        public int Id { get; set; } = 0;
        public int Capacity { get; set; } = 5;
        public bool isrunnig { get; set; } = false;

        public Scene RoomScene;
        public NetworkConnection Host;
        public List<NetworkConnection> Players;
    }
}