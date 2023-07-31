using System;
using onlinetutorial.controllers;
using onlinetutorial.data;

namespace onlinetutorial.interfaces.PlayerAttack
{
    public class AttackStateMachine : StateMachine
    {
        public  Player Player { get; }
        public ResuableAttackData ReusableAttackData { get; }

        public  punchState PunchState { get; }
        public  deadState DeadState { get; }

        
        public  attackidleState AttackIdle { get; }
        
        public  crospunchState CrospunchState { get; }

  

        public AttackStateMachine(Player player)
        {
            Player = player;
            ReusableAttackData = new ResuableAttackData();
            DeadState = new deadState(this);
            PunchState = new punchState(this);
            AttackIdle = new attackidleState(this);
            CrospunchState = new crospunchState(this);

        }
    }
}