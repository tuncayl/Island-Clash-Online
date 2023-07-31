using System.Collections.Generic;
using Keys;
using onlinetutorial.controllers;
using onlinetutorial.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace onlinetutorial.MVVM
{
    public class RoomListViewModel : MonoBehaviour,IRoomListViewModel
    {
        [SerializeField]  private RoomListModel _roomListModel;
        
        public void RoomSetPlayers(ref List<RoomData> roomDatas, ref Transform root)
        {
            root.ClearRoot();
            root.AddRoot(ref roomDatas,_roomListModel.RoomPrefabValue);
        }
    }
    
    public interface IRoomListViewModel
    {
        void RoomSetPlayers(ref List<RoomData> roomDatas,ref Transform root);
    }
}