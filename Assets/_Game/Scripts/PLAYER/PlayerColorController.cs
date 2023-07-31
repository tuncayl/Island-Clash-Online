using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS.Playercommands;
using DG.Tweening;
using FishNet.Managing.Timing;
using FishNet.Object;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public class PlayerColorController : NetworkBehaviour
    {
        #region SelfVariables

        [field:SerializeField]private Player _Player { get; set; }

        [SerializeField] private float DisolveTime = 10;

        private SkinnedMeshRenderer _renderer;

        #endregion


        #region UnityMethod

        private void Awake()
        {
            _renderer = GetComponent<SkinnedMeshRenderer>();
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            UnSubsicreEvent();
        }

        #endregion

        #region OtherMethod

        public void INIT()
        {
            if (_Player.ResuableGeneralData.Chibicolor == chibicolor.yellow) return;
            SendChangeColor(_Player.ResuableGeneralData.Chibicolor);
        }

        private void SendChangeColor(chibicolor chibicolor)
        {
            Debug.Log(chibicolor);
            //local
            OnChangeColor(chibicolor);

            //send server
            ServerChangeColor(chibicolor);
        }

        private void OnChangeColor(chibicolor _chibicolor)
        {
            Debug.Log(_chibicolor+" ON change color");
            new ChangeColorCommand(ref _renderer, ref _Player.ResuableGeneralData, _chibicolor).execute();
        }
       

        private void OnPlayerDead() =>
            new ColorDisolveCommand(ref _renderer, DisolveTime, _Player.ResuableGeneralData.Chibicolor)
                .execute();

        private void OnPlayerBirth() =>
            new ColorReDisolveCommand(ref _renderer, DisolveTime / 2.5f, _Player.ResuableGeneralData.Chibicolor,
                OnChangeColor).execute();

        #endregion

        #region ServerMethods

        public override void OnStartClient()
        {
            base.OnStartClient();
            SubscireEvent();
        }

        [ServerRpc(RequireOwnership = true)]
        private void ServerChangeColor(chibicolor _chibicolor) => ObserverChangeColor(_chibicolor);


        [ObserversRpc(ExcludeOwner = true, BufferLast = true)]
        private void ObserverChangeColor(chibicolor _chibicolor) => OnChangeColor(_chibicolor);

        #endregion

        #region SubscireEvent

        private void SubscireEvent()
        {
            if (IsOwner)
            {
            }

            if (IsClient)
            {
                _Player._PlayerLocalSignals.OnDead += OnPlayerDead;
                _Player._PlayerLocalSignals.OnBirth += OnPlayerBirth;
            }
        }

        private void UnSubsicreEvent()
        {
            if (IsOwner)
            {
            }

            if (IsClient)
            {
                _Player._PlayerLocalSignals.OnDead -= OnPlayerDead;
                _Player._PlayerLocalSignals.OnBirth -= OnPlayerBirth;
            }
        }

        #endregion
    }
}