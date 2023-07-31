using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace onlinetutorial.interfaces
{
    public interface IState
    {
        public void Enter();

        public void Exit();

        public void Input(InputData ınputData);

        public void Update(float tickdelta);

        public void PhysicsUpdate(float tick = 60);

 
    }
}