using UnityEngine;
using UnityEngine.InputSystem;

namespace onlinetutorial.interfaces.PlayerAttack
{
    public class punchState : AttackState
    {
        public punchState(AttackStateMachine attackStateMachine) : base(attackStateMachine)
        {
        }

        private float endtime = default;
        public override void Enter()
        {
            base.Enter();
            statemachine.Player.Animator.SetTrigger("punch");
            endtime = attackadata.endtime+Time.time;
            resuabledata.nextAttackrate = Time.time + attackadata.AttackRate;

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(float tickdelta)
        {
            base.Update(tickdelta);
            AnimationActiveLayer(1,1);
            
            if (Time.time > endtime)
            {
                statemachine.ChangeState(statemachine.AttackIdle);
                return;
            }

        }

        protected override void AttackStart()
        {
           if(Time.time < endtime-0.50f) return;
           statemachine.ChangeState(statemachine.CrospunchState);
            
        }
    }
}