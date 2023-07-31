using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class landigState : MovementState
    {

        public landigState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
        {
        }

        public override void Enter()
        {
            UpdateStartTime = Time.time + 0.25f;

            StartAnimationPlayer(statemachine.Player._Animatiodata.JumpingDownParameterHash);
            statemachine.Player.Rigidbody.drag = movementData.Drag;
            base.Enter();
            SlopeControl();

        }

        public override void Exit()
        {
            base.Exit();

        }

        public override void Update(float tickdelta)
        {
            if(UpdateStartTime>Time.time) return;
            base.Update(tickdelta);
            if (statemachine.ReusableMoveData.refmove == Vector2.zero)
            {
                StartAnimationPlayer(statemachine.Player._Animatiodata.IdleParameterHash);
                statemachine.ChangeState(statemachine.idleState);
                return;
            }
            StartAnimationPlayer(statemachine.Player._Animatiodata.MovingParameterHash);
            statemachine.ChangeState(reusabledata.RefSpeed==1f?statemachine.WalkingState:statemachine.RunnigState);
        }

        public override void PhysicsUpdate(float tick)
        {
            base.PhysicsUpdate(tick);
        }
    }
}