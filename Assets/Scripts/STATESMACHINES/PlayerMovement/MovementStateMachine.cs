using System;
using FishNet.Component.Animating;
using onlinetutorial.controllers;
using onlinetutorial.data;
using onlinetutorial.interfaces.Movement;
using UnityEngine;

namespace onlinetutorial.interfaces
{
    public class MovementStateMachine : StateMachine
    {
        
        public  Player Player { get; }
        public ReusableMoveData ReusableMoveData { get; }
        

        public IdleState idleState { get; }
        public WalkingState WalkingState { get; }
        public RunnigState RunnigState { get; }
        public  jumpState JumpState { get; }
        public  fallingState FallingState { get; }
        
        public  landigState LandigState { get; }

        public  deadState DeadState { get; }
        
        public  birthState BirthState { get; }

        public MovementStateMachine(Player player)
        {
            Player = player;
            ReusableMoveData = new ReusableMoveData();
            idleState = new IdleState(this);
            WalkingState = new WalkingState(this);
            RunnigState = new RunnigState(this);
            JumpState = new jumpState(this);
            FallingState = new fallingState(this);
            LandigState = new landigState(this);
            DeadState = new deadState(this);
            BirthState = new birthState(this);

        }
    }
    
}