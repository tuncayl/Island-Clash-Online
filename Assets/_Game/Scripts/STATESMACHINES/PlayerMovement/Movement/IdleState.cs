using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class IdleState: MovementState
    {
        public IdleState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(float tickdelta)
        {
            base.Update(tickdelta);
            CheckSpeed(tickdelta);
            AirControl();
            SlopeControl();
            if (statemachine.ReusableMoveData.refmove == Vector2.zero)
            {
                return;
            }
            OnMove();
        }

        public override void PhysicsUpdate(float tick)
        {
            base.PhysicsUpdate(tick);
        }

       
    }
}