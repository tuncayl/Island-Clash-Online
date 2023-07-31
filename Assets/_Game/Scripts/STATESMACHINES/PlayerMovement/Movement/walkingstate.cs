using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class WalkingState: MovementState
    {
        public WalkingState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
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
            if (reusabledata.RefSpeed > 1)
            {
                statemachine.ChangeState(statemachine.RunnigState);
                return;
            }

            if (reusabledata.refmove != Vector2.zero)
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