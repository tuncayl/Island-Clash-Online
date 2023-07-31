using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class birthState : MovementState
    {
        private float StateTimer = 0;
        public birthState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
        {
        }

        public override void Enter()
        {
            StateTimer = Time.time + 1.5f;
            Debug.Log("birth state");
            if(statemachine.Player.IsServer )return;
           StartAnimationPlayer(statemachine.Player._Animatiodata.BirthParameterHash);
        }


        public override void Update(float tickdelta)
        {
            //base.Update(tickdelta);
            if(StateTimer>Time.time)return;
            if(!statemachine.Player.IsServer)StartAnimationPlayer(statemachine.Player._Animatiodata.IdleParameterHash);
            statemachine.ChangeState(statemachine.idleState);
        }

        public override void Exit()
        {
            //base.Exit();
            PlayerSignal.Instance.OnPlayerSpawned.Invoke();
            statemachine.Player._attackStateMachine.ChangeState(statemachine.Player._attackStateMachine.AttackIdle);

        }
    }
}