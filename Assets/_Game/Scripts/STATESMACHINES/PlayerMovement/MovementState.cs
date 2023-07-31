using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using onlinetutorial.controllers;
using onlinetutorial.Signals;
using onlinetutorial.data;
using UnityEngine;
using UnityEngine.InputSystem;


namespace onlinetutorial.interfaces
{
    public class MovementState : IState
    {
        protected MovementStateMachine statemachine;

        protected readonly MovementData movementData;

        protected readonly ReusableMoveData reusabledata;
        
        protected PhysicsScene _physicsScene => statemachine.Player._PlayerPhsicsController.physicsScene;

        protected float UpdateStartTime=0;

        private Action OnjumpAction = delegate() { };
        
        

        public MovementState(MovementStateMachine movementStateMachine)
        {
            statemachine = movementStateMachine;
            movementData = statemachine.Player.Data.movementData;
            reusabledata = statemachine.ReusableMoveData;
        }


        #region interfaceMethods

        public virtual void Enter()
        {
            //Debug.Log("state:  " + GetType().Name);
            SubcireInput();
        }

        public virtual void Exit()
        {
            UnSubscireInput();
        }

        public virtual IState GetCurrentState()
        {
            return this;
        }

        public virtual void Input(InputData md)
        {
            readInput(md);
        }

        public virtual void Update(float tickdelta)
        {
            CalculateDirection(tickdelta);
            SpeedControl();
        }

        public virtual void PhysicsUpdate(float tick = 60)
        {
            move(tick);
        }
    

        #endregion

        #region OtherMethods

        private void readInput(InputData md)
        {
            statemachine.ReusableMoveData.Movelerp =
                md.move;
            reusabledata.cameraAngle = md.camangle;
            if (md.jump) OnjumpAction?.Invoke();
            reusabledata.RefSpeed = md.sprint;
        }


        protected virtual void move(float tick, float air = 1f)
        {
            statemachine.Player.Rigidbody.AddForce
            ((reusabledata.moveVec.normalized * (movementData.PMoveSpeed * reusabledata.moveSpeed) * 0.4f * 10f * air),
                ForceMode.Force);
        }

        protected virtual void SlopeMove()
        {
            statemachine.Player.Rigidbody.AddForce
            (GetSlopeDirection() * movementData.PMoveSpeed *reusabledata.moveSpeed * 0.4f * 10f,
                ForceMode.Force);
        }

        private void CalculateDirection(float tickdelta)
        {
            #region MoveDirection

            //lerp Move Value
            reusabledata.refmove = Vector3.Lerp(reusabledata.refmove,
                reusabledata.Movelerp,
                movementData.MoveLerpSmoth * tickdelta);


            //check the size of our input value.
            if (reusabledata.refmove.magnitude >= 0.1f)
            {
                //use the Atan2 function to find our target angle.convert it to degrees with mathf.rad2dg and add it to the y-axis of the camera.
                reusabledata.currentAngle =
                    Mathf.Atan2(reusabledata.refmove.x, reusabledata.refmove.y) * Mathf.Rad2Deg +
                    reusabledata.cameraAngle;

                //child rotate y axixs currentangle;
                statemachine.Player.transform.rotation =
                    Quaternion.Slerp(statemachine.Player.transform.rotation,
                        Quaternion.Euler(0, reusabledata.currentAngle, 0), movementData.rotationSmoothTime * tickdelta);

                //equate our motion vector such that it is the front of the targeted angle.
                reusabledata.moveVec = Quaternion.Euler(0, reusabledata.currentAngle, 0) * Vector3.forward;
                reusabledata.moveVec.y = 0;
            }
            else
            {
                reusabledata.refmove = Vector2.zero;
                reusabledata.moveVec = Vector3.zero;
                reusabledata.RefSpeed = 1;
            }
       
            #endregion

            //Anim set Blend speed value
            statemachine.Player.Animator.SetFloat
            ("speed", reusabledata.refmove.magnitude
                      * reusabledata.speed, 2.5f*tickdelta,tickdelta);
        }

        private void SpeedControl()
        {
            Vector3 vel = new Vector3(
                statemachine.Player.Rigidbody.velocity.x, 0f,
                statemachine.Player.Rigidbody.velocity.z);

            // limit velocity if needed
            if (vel.magnitude > reusabledata.moveSpeed)
            {
                Vector3 limitedVel = vel.normalized * reusabledata.moveSpeed;
                statemachine.Player.Rigidbody.velocity =
                    new Vector3(limitedVel.x,
                        statemachine.Player.Rigidbody.velocity.y,
                        limitedVel.z);
            }
        }

        #endregion

        #region ResuableMethods

        protected virtual void OnMove()
        {
            statemachine.ChangeState(reusabledata.RefSpeed == 1f
                ? statemachine.WalkingState
                : statemachine.RunnigState);
        }

        protected virtual void CheckSpeed(float tickdelta)
        {
            //lerp Move Speed 
            reusabledata.speed = Mathf.Lerp(reusabledata.speed,
                reusabledata.RefSpeed,
                tickdelta * movementData.SpeedLerp);
        }


        protected void StartAnimationPlayer(int animationHash)
        {
            if (statemachine.Player.IsServer is false) statemachine.Player.Animator.SetTrigger(animationHash);

        }

        protected void ResetAnimationPlayer(int animationHash)
        {
            if (statemachine.Player.IsServer is false) statemachine.Player.Animator.ResetTrigger(animationHash);

        }

        protected virtual Vector3 GetSlopeDirection()
        {
            return Vector3.ProjectOnPlane(reusabledata.moveVec,
                reusabledata.SlopeHit.normal).normalized;
        }

        protected virtual bool OnSlope()
        {
            if (_physicsScene.Raycast(statemachine.Player.transform.position,
                    Vector3.down, out reusabledata.SlopeHit,
                    movementData.PlayerHeight * 0.5f + 0.2f))
            {
                float angle = Vector3.Angle(Vector3.up, reusabledata.SlopeHit.normal);
                return angle < movementData.MaxSlopeAngle && angle != 0;
            }

            return false;
        }

        protected virtual bool IsGroundOverlap()
        {
            return 1<=_physicsScene.OverlapSphere(
                statemachine.Player.GroundPoint.position, movementData.radius,new Collider[5], movementData.groundMask,QueryTriggerInteraction.Collide);
        }

        protected virtual bool IsGround()
        {
            RaycastHit outhit;

            return _physicsScene.Raycast(statemachine.Player.transform.position,
                Vector3.down, movementData.PlayerHeight * 0.5f + 0.2f, movementData.groundMask);
        }

        protected virtual void AirControl()
        {
            if (statemachine.Player.PredictionManager.IsReplaying()) return;
            if (!IsGround())
            {
                if (statemachine.Player.Rigidbody.velocity.y >= 0) return;
                if (Mathf.Abs(statemachine.Player.Rigidbody.velocity.y) < 1.5) return;

                statemachine.ChangeState(statemachine.FallingState);
            }
        }

        protected virtual void SlopeControl() => statemachine.Player.Rigidbody.useGravity = !OnSlope();

        protected virtual void GravityOn() => statemachine.Player.Rigidbody.useGravity = true;

        #endregion


        #region CallbacksMethod

        protected virtual void SubcireInput()
        {
            OnjumpAction += OnJump;
            statemachine.Player._PlayerLocalSignals.OnDead += OnDead;
        }

        protected virtual void UnSubscireInput()
        {

            OnjumpAction -= OnJump;
            statemachine.Player._PlayerLocalSignals.OnDead -= OnDead;

        }

        protected virtual void OnJump()
        {
            if (reusabledata.nextjump) return;
            if (!IsGroundOverlap()) return;
            statemachine.ChangeState(statemachine.JumpState);
        }

        protected virtual void OnDead()=>   statemachine.ChangeState(statemachine.DeadState);

        protected virtual void OnBirth() => statemachine.ChangeState(statemachine.BirthState);
        
        [Obsolete]
        protected  virtual void OnRunPerformed(InputAction.CallbackContext context)
        {
            reusabledata.RefSpeed = context.ReadValue<float>() + 1;
        }
        [Obsolete]
        protected virtual void OnRunCancelled(InputAction.CallbackContext context)
        {
            reusabledata.RefSpeed = 1;
        }

        #endregion
    }
}