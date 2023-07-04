using System;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class CoreGameSignals: MonoSingleton<CoreGameSignals>
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public  UnityAction OnConnectedClient=delegate() {  };
        public  UnityAction OnConnectedServer=delegate (){  };
        
        public  UnityAction OnGameEnd=delegate {  };
    }
}