using System.Collections.Generic;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class UiMainSignals: MonoSingleton<UiMainSignals>
    {
       public UnityAction<int> OnChangeGameCoutDown=delegate(int arg0) {  };
       public UnityAction<int> OnChangeSpawnCoutDown=delegate(int arg0) {  };
       public  UnityAction<bool> OnActiveSpawnCoutDown=delegate(bool arg0) {  };


    }
}