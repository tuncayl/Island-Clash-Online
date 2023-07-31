using UnityEngine;
using UnityEngine.InputSystem;

namespace onlinetutorial.interfaces.PlayerAttack
{
    public class crospunchState:AttackState
    {
        
        public crospunchState(AttackStateMachine attackStateMachine) : base(attackStateMachine)
        {
            
        }
        private float endtime = default;
        public override void Enter()
        {
            base.Enter();
            statemachine.Player.Animator.SetTrigger("crospunch");
            endtime = Time.time + 1f;

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(float tickdelta)
        {
            base.Update(tickdelta);
            if (Time.time > endtime)
            {
                statemachine.ChangeState(statemachine.AttackIdle);
                return;
            }
            
        }

        protected override void AttackStart()
        {
            
        }
    }
}