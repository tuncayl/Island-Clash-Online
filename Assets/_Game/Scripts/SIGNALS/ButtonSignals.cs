using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class ButtonSignals: MonoSingleton<ButtonSignals>
    {
        public  UnityAction OnJoinRoom=delegate (){  };
        
        public  UnityAction OnCreateRoom=delegate (){  };
        
        public  UnityAction OnSetting=delegate (){  };
        
        public  UnityAction OnRoomBack=delegate() {  };

        public  UnityAction OnChamgeName=delegate {  };
        
        public  UnityAction OnRoomList=delegate {  };
        
        public  UnityAction OnStartCount=delegate {  };
        
        public  UnityAction OnMain=delegate {  };
        
        public  UnityAction OnEndGame=delegate {  };
        
        public  UnityAction OnMenuLoad=delegate {  };
        
    }
}