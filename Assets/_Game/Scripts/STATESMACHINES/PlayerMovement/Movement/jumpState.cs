using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class jumpState : MovementState
    {

        public jumpState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
        {
        }

        public override void Enter()
        {
            UpdateStartTime = Time.time + 0.25f;
            statemachine.Player.Rigidbody.drag = 0;
            statemachine.ReusableMoveData.RefSpeed = 1;
            statemachine.Player.Animator.SetFloat
                ("speed", 0);

            StartAnimationPlayer(statemachine.Player._Animatiodata.JumpingUpParameterHash);
            GravityOn();
            jump();
            base.Enter();

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(float tickdelta)
        {
            base.Update(tickdelta);
            if (UpdateStartTime > Time.time) return;


            if (statemachine.Player.Rigidbody.velocity.y < -0.1f is false)
                return;
            statemachine.ChangeState(statemachine.FallingState);
        }

        public override void PhysicsUpdate(float tick)
        {
            move(tick, movementData.airMultiplier);
        }

        private void jump()
        {
            reusabledata.nextjumprate = Time.time + 0.7f;
            statemachine.Player.Rigidbody.velocity =
                new Vector3(statemachine.Player.Rigidbody.velocity.x,
                    Mathf.Sqrt(movementData.jumpboost * 2f * 9.81f),
                    statemachine.Player.Rigidbody.velocity.z);
        }
    }
}