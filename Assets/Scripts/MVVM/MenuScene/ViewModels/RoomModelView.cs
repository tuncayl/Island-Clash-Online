using System.Collections.Generic;
using Keys;
using onlinetutorial.controllers;
using onlinetutorial.Extensions;
using TMPro;
using UnityEngine;

namespace onlinetutorial.MVVM
{
    public class RoomModelView : MonoBehaviour,IRoomViewModel
    {
        [SerializeField]  private RoomModel _model;


        public void RoomSetPlayers(ref List<UserInfo> userDatas,ref Transform root)
        {
            root.ClearRoot();
            root.AddRoot(ref userDatas,_model.UserPrefabValue);
        }
    }
    
    public interface IRoomViewModel
    {
        void RoomSetPlayers(ref List<UserInfo> userDatas,ref Transform root);

    }
}