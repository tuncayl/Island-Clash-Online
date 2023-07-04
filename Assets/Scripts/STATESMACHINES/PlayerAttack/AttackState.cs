using System;
using FishNet.Object;
using onlinetutorial.data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace onlinetutorial.interfaces.PlayerAttack
{
    public class AttackState : IState
    {
        protected AttackStateMachine statemachine;

        protected readonly ResuableAttackData resuabledata;

        protected readonly AttackData attackadata;

        private Action OnAttackAction = delegate() { };
        public AttackState(AttackStateMachine attackStateMachine)
        {
            statemachine = attackStateMachine;
            resuabledata = statemachine.ReusableAttackData;
            attackadata = statemachine.Player.Data.attackData;
        }

        #region interfaceMethods

        public virtual void Enter()
        {
            SubcireInput();
        }

        public virtual void Exit()
        {
            UnSubscireInput();
        }

        public virtual void Input(InputData md)
        {
            if (md.Attack) OnAttackAction?.Invoke();
        }

        public virtual void Update(float tickdelta)
        {
            
        }

        public virtual void PhysicsUpdate(float tick = 60)
        {
            
        }

      

        #endregion

        #region ReusableMethods

        private void OnPlayerDead() => statemachine.ChangeState(statemachine.DeadState);

        protected virtual void SubcireInput()
        {
            OnAttackAction += AttackStart;

            statemachine.Player.AnimationEvents.punchEvent += PunchEvent;
            statemachine.Player._PlayerLocalSignals.OnDead  += OnPlayerDead;
        }

        protected virtual void UnSubscireInput()
        {
            OnAttackAction -= AttackStart;
            statemachine.Player.AnimationEvents.punchEvent -= PunchEvent;
            statemachine.Player._PlayerLocalSignals.OnDead -= OnPlayerDead;

        }

        protected virtual void AttackStart()
        {
            if(resuabledata.nextAttack)return;
            statemachine.ChangeState(statemachine.PunchState);
            
        }

     

        protected virtual void AnimationActiveLayer(int index,float value)
        {
            var m_currentLayerWeight = statemachine.Player.Animator.GetLayerWeight(1);
            m_currentLayerWeight =Mathf.SmoothDamp(m_currentLayerWeight,value, ref resuabledata._currentLayerWeightVelocity,attackadata.animTransSmoothTime);
            statemachine.Player.Animator.SetLayerWeight(1,m_currentLayerWeight);
        }

        protected virtual void PunchEvent()
        {
           statemachine.Player._playerAttackController.Punch();
           
        }

        #endregion
    }
}