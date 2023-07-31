using System;
using FishNet.CodeAnalysis.Annotations;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers.Room
{
    public abstract class SyncCounter : NetworkBehaviour
    {
        #region SelfVariables

        [SyncObject]  readonly SyncTimer _timeRemaining = new SyncTimer();

        protected int Remaning = 3;

        protected bool Finished = false;

        protected int minute => Mathf.FloorToInt(_timeRemaining.Remaining);

        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            Subscire();
        }

        private void OnDisable()
        {
            UnSubscire();
        }
        
        [OverrideMustCallBase(BaseCallMustBeFirstStatement=true)]
        protected virtual void Update()
        { 
            if (Finished is true) return;
            _timeRemaining.Update(Time.deltaTime);
        }

        #endregion

        #region MainMethods

        private void OnFinishedTimer()
        {
            Finished = false;

            if (IsServer)
            {
                OnCompleteServer();
            }

            if (IsClient&& IsOwner)
            {
                OnCompletedOwner();
            }

            if (IsClient)
            {
                OnCompleteClient();

            }
        }

        private void _timeRemaining_OnChange(SyncTimerOperation op, float prev, float next, bool asServer)
        {
            switch (op)
            {
                case SyncTimerOperation.Finished:
                    OnFinishedTimer();
                    break;
            }
        }

        #endregion


        #region ServerMethods

        public override void OnStartServer()
        {
            base.OnStartServer();
            _timeRemaining.StartTimer(Remaning, sendRemainingOnStop: true);
        }

        #endregion


        #region ResuableMethods

        protected virtual void OnCompleteServer()
        {
            Debug.Log("COMPLETED SERVER" + GetType().Name);
        }

        protected virtual void OnCompletedOwner()
        {
            Debug.Log("COMPLETED OWNER CLIENT" + GetType().Name);
        }
        protected virtual void OnCompleteClient()
        {
            Debug.Log("COMPLETED CLIENT" + GetType().Name);
        }
        #endregion

        #region SubscireMethods

        private void Subscire()
        {
            _timeRemaining.OnChange += _timeRemaining_OnChange;
        }

        private void UnSubscire()
        {
            _timeRemaining.OnChange -= _timeRemaining_OnChange;
        }

        #endregion
    }
}