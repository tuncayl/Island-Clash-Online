using UnityEngine;

namespace onlinetutorial.interfaces.PlayerAttack
{
    public class attackidleState: AttackState
    {
        public attackidleState(AttackStateMachine attackStateMachine) : base(attackStateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            statemachine.Player.Animator.ResetTrigger("punch");
            statemachine.Player.Animator.ResetTrigger("crospunch");

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(float tickdelta)
        {
            base.Update(tickdelta);
            AnimationActiveLayer(1,0);

        }

        public override void PhysicsUpdate(float tick)
        {
            base.PhysicsUpdate(tick);
        }
    }
}