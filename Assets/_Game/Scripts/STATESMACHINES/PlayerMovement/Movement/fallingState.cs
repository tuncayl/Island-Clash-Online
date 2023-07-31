using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class fallingState : MovementState
    {
        public fallingState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
        {
        }

        public override void Enter()
        {
            statemachine.Player.Rigidbody.drag = 0;
            StartAnimationPlayer(statemachine.Player._Animatiodata.FallingParameterHash);
            SlopeControl();
            GravityOn();
        }

        public override void Exit()
        {
            base.Exit();
            ResetAnimationPlayer(statemachine.Player._Animatiodata.FallingParameterHash);


        }

        public override void Update(float tickdelta)
        {
            base.Update(tickdelta);
            if (IsGround())
            {
                statemachine.ChangeState(statemachine.LandigState);
            }
        }

        public override void PhysicsUpdate(float tick)
        {
            move(tick, movementData.airMultiplier);
        }
    }
}