using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Timing;
using FishNet.Object;
using onlinetutorial.Extensions;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public class PlayerPhysicsController : NetworkBehaviour
    {
        #region SelfVariables

        private Player _Player { get; set; }
        [field: SerializeField] public LayerMask _Playermask { get; private set; }
        [field: SerializeField] public LayerMask _WithOutPlayerMask { get; private set; }

        public CapsuleCollider _CapsuleCollider { get; private set; }

        public PhysicsScene physicsScene;

        #endregion


        #region UnityMethod

        private void Awake()
        {
            _CapsuleCollider = transform.GetComponent<CapsuleCollider>();
            _Player = transform.parent.parent.GetComponent<Player>();
            physicsScene = gameObject.scene.GetPhysicsScene();
        }

        #endregion

        #region ServerMethods
        
        [ObserversRpc]
        public void ObserverPosition(Vector3 positon) => _Player.transform.position = positon;

        #endregion

        #region OtherMethod
   

        private void OnPlayerDead() => this.gameObject.layer = _WithOutPlayerMask.ToLayer();
   
        private void OnPlayerBirth()=> this.gameObject.layer =  _Playermask.ToLayer();


        private void OnGameEnd()
        {
            this.gameObject.layer = _WithOutPlayerMask.ToLayer();
            _Player.Rigidbody.isKinematic = true;

        }
     

        #endregion

        #region ServerMethods

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            SubscireEvent();
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            UnSubsicreEvent();
        }

        #endregion

        #region SubscireEvent

        private void SubscireEvent()
        {
            if (IsOwner) CoreGameSignals.Instance.OnGameEnd += OnGameEnd;
            _Player._PlayerLocalSignals.OnDead += OnPlayerDead;
            _Player._PlayerLocalSignals.OnBirth += OnPlayerBirth;
            _Player._PlayerLocalSignals.OnGameEnd += OnGameEnd;
        }


        private void UnSubsicreEvent()
        {
            if (IsOwner) CoreGameSignals.Instance.OnGameEnd -= OnGameEnd;

            _Player._PlayerLocalSignals.OnDead -= OnPlayerDead;
            _Player._PlayerLocalSignals.OnBirth -= OnPlayerBirth;
            _Player._PlayerLocalSignals.OnGameEnd -= OnGameEnd;

        }

        #endregion
    }
}