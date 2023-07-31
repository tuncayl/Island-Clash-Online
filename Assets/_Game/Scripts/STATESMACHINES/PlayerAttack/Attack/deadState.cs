using UnityEngine;

namespace onlinetutorial.interfaces.PlayerAttack
{
    public class deadState : AttackState
    {
        public deadState(AttackStateMachine attackStateMachine) : base(attackStateMachine)
        {
        }

        public override void Enter()
        {
            //base.Enter();
            
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(float tickdelta)
        {
            //base.Update(tickdelta);
        }
    }
}