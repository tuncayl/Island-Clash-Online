using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class RunnigState: MovementState
    {
        public RunnigState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
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
            SlopeControl();
            AirControl();

            if (reusabledata.RefSpeed == 1f)
            {
                statemachine.ChangeState(statemachine.WalkingState);
                return;
            }
            
            if (statemachine.ReusableMoveData.refmove != Vector2.zero)
            {
                return;
            }
            statemachine.ChangeState(statemachine.idleState);
            
        }

        public override void PhysicsUpdate(float tick)
        {
            if (OnSlope())
            {
                SlopeMove();
                return;
            }
            base.PhysicsUpdate(tick);
        }
    }
}