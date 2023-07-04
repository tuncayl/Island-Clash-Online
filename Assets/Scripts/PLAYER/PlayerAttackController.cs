using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS.Playercommands;
using FishNet.Managing.Timing;
using FishNet.Object;
using onlinetutorial.data;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public class PlayerAttackController : NetworkBehaviour
    {
        #region SelfVariables

        private Player _Player;
        

        #endregion


        #region UnityMethod

        private void Awake()
        {
            _Player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            SubscireEvent();
        }

        private void OnDisable()
        {
            UnSubsicreEvent();
        }

        #endregion

        #region OtherMethod

        [Client]
        public void Punch()
        {
            PreciseTick pt = base.TimeManager.GetPreciseTick(base.TimeManager.LastPacketTick);
            ServerPunch(pt, IslandSignals.Instance.OnGetIslandId.Invoke());
        }

        [ServerRpc]
        private void ServerPunch(PreciseTick pt, int roomid)
        {
            base.RollbackManager.Rollback(pt, base.OwnerId, roomid);
            new AttackCommand(_Player._PlayerPhsicsController,_Player.AttackPoint.position,ref _Player.Data.attackData.AttackPower).execute();
            base.RollbackManager.Return(base.OwnerId, roomid);
            return;
        }

        #endregion

        #region SubscireEvent

        private void SubscireEvent()
        {
        }

        private void UnSubsicreEvent()
        {
        }

        #endregion
    }
}