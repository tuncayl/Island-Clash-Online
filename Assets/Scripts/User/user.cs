using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Keys;
using onlinetutorial.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace onlinetutorial.controllers
{
    
  
    public class user : NetworkBehaviour
    {
        #region SelfVariables

        [SyncVar] public string name = default;

        [SyncVar] public int id  = -1;

        #endregion


        #region UnityMethods

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Subscire();
        }

        private void OnDisable()
        {
            UnSubscire();
        }

   
        private void Awake()
        {
            name = PlayerPrefs.GetString("name", "chibi"+Random.Range(1000,9999));

        }

 
        public override void OnStartClient()
        {
            base.OnStartClient();
            if(IsOwner is false) return;

            UserSignals.Instance.OnChangeName(name);

        }
        #endregion

        #region OtherMethods
        
        [ServerRpc(RequireOwnership = false)]
        private void ChangeName(string _newname)=>   name = _newname;
        

        private void OnAddUser(ref List<UserInfo> Users)=>  Users.Add(new UserInfo(name));



        private UserInfo OnGetUserData() => new UserInfo(name);

        private user OnGetUser() => this;
        
        
        #endregion

        #region NetworkMethods

        private int OnGetRoomId() => id;
        

        public override void OnStopServer()
        {
            base.OnStopServer();
            Debug.Log("DISCONNECT");
            UserSignals.Instance.OnDisconnected?.Invoke(id,this.Owner);
        }

        #endregion

        #region SubscireMethods

        private void Subscire()
        {
            if (IsServer is true)
            {
                UserSignals.Instance.OnGetRoomId += OnGetRoomId;

            }

            if (IsOwner is true)
            {
                UserSignals.Instance.OnChangeName += ChangeName;
                UserSignals.Instance.OnGetUser += OnGetUser;
                UserSignals.Instance.OnGetUserData += OnGetUserData;
            }

            UserSignals.Instance.OnRoomAdduser += OnAddUser;

        }

        private void UnSubscire()
        {
            if (IsServer is true)
            {
                UserSignals.Instance.OnGetRoomId -= OnGetRoomId;

            }

            if (IsOwner is true)
            {
                UserSignals.Instance.OnGetUser -= OnGetUser;

                UserSignals.Instance.OnChangeName -= ChangeName;
                UserSignals.Instance.OnGetUserData -= OnGetUserData;

            }
            UserSignals.Instance.OnRoomAdduser -= OnAddUser;

        }

        #endregion
    }
}