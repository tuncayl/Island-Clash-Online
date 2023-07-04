using System.Collections.Generic;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using  Customdelegates.v1;
using Keys;
using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class UiMenuSignals: MonoSingleton<UiMenuSignals>
    {
        
        public UnityRefAction<List<UserInfo>> OnUiListedPlayer=delegate(ref List<UserInfo> item1) {  };
        
        public UnityRefAction<List<RoomData>> OnUiListedRoom=delegate(ref List<RoomData> item1) {  };
        
        public  UnityAction<int> OnChangeCounter=delegate(int arg0) {  };
        


    }
}