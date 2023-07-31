using System;
using FishNet;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.controllers
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        #region SelfVariables

        public UnityAction punchEvent=delegate (){  };


        #endregion
        private void Start()
        {
        }


        #region AttackEvents
        
        
        
        
        public void PunchEvent()
        {
            if(enabled is false) return;
            punchEvent?.Invoke();
        }
        
        #endregion
        
    }
}