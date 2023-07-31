using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.interfaces.Movement
{
    public class deadState : MovementState
    {

        public deadState(MovementStateMachine movementStateMachine) : base(movementStateMachine)
        {
        }

        public override void Enter()
        {
            statemachine.Player._PlayerLocalSignals.OnBirth  += OnBirth;
            if(statemachine.Player.IsServer )return;
            StartAnimationPlayer(statemachine.Player._Animatiodata.DeadParameterHash);
        }


        public override void Update(float tickdelta)
        {
            //base.Update(tickdelta);
        }

        public override void Exit()
        {
          // base.Exit();
          statemachine.Player._PlayerLocalSignals.OnBirth -= OnBirth;

        }
    }
}