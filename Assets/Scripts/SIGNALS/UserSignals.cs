using System;
using System.Collections.Generic;
using FishNet.Connection;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using Customdelegates.v1;
using Keys;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class UserSignals: MonoSingleton<UserSignals>
    {

        public Func<NetworkConnection> OnGetconnection = delegate() { return default; };
        
        public UnityAction<NetworkConnection,int> OnSetMatchid=delegate(NetworkConnection arg0,int arg1) {  };
        
        public UnityAction OnConnected=delegate {  };
        
        public UnityAction<int,NetworkConnection> OnDisconnected=delegate (int roomid,NetworkConnection con){  };
        public Func<int> OnGetRoomId= delegate() { return default;};
        
        public UnityAction<string> OnChangeName=delegate(string arg0) {  };
        
        public UnityAction<int> OnChangeID=delegate(int arg0) {  };

        public  UnityRefAction<List<UserInfo>> OnRoomAdduser=delegate(ref List<UserInfo> item1) {  };
        
        public  Func<SpawnArgs> OnGetUserspawnargs= delegate() { return default;};
        
        public  Func<UserInfo> OnGetUserData= delegate { return default;};
        
        public  Func<user> OnGetUser= delegate { return default;};


    }
} 