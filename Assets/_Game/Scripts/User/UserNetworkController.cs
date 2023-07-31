using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Keys;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;


namespace onlinetutorial.controllers
{
    [RequireComponent(typeof(user))]
    public class UserNetworkController : NetworkBehaviour
    {
        #region SelfVariables

        private user User;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            User = GetComponent<user>();
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Subscire();
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            UnSubscire();
        }

        #endregion

        #region OtherMethods

        private void OnSetMatchid(NetworkConnection _connection, int _id)
        {

            if (_connection != this.Owner) return;
            Debug.Log("CHANGED ID "+_id);
            User.id = _id;
        }

    

        private NetworkConnection OngetConnection()
        {
            return this.Owner;
        }


        private SpawnArgs OnGetUserspawnargs() => new SpawnArgs(this.transform, this.Owner, User.id);

        #endregion

        #region SubscireMethods

        private void Subscire()
        {
            if (IsServer is true)
            {
                UserSignals.Instance.OnSetMatchid += OnSetMatchid;
            }

            if (IsOwner is true)
            {
                UserSignals.Instance.OnGetconnection += OngetConnection;
                UserSignals.Instance.OnGetUserspawnargs += OnGetUserspawnargs;
            }
        }

        private void UnSubscire()
        {
            if (IsServer is true)
            {
                Debug.Log("UNSUBNETWORK");
                UserSignals.Instance.OnSetMatchid -= OnSetMatchid;
            }

            if (IsOwner is true)
            {
                UserSignals.Instance.OnGetconnection -= OngetConnection;
                UserSignals.Instance.OnGetUserspawnargs -= OnGetUserspawnargs;
            }
        }

        #endregion
    }
}