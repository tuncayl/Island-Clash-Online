using System;
using onlinetutorial.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class PanelSignals : MonoSingleton<PanelSignals>
    {
        public  UnityAction<bool>OnChangeRaycaster=delegate(bool arg0) {  };

        public UnityAction<menus> OnMenuChange= delegate(menus arg0) {  };
        
        public UnityAction<menus,bool> OnButtonClicked= delegate(menus arg0,bool arg1) {  };
        
        public  UnityAction OnCloseCurrentPanel=delegate {  };
    }
}