using UnityEngine.Events;

namespace onlinetutorial.Signals
{
    public class PlayerLocalSignals
    {
        //GeneralSignals
        public  UnityAction OnGameEnd=delegate() {  };



        //HealtSignals
        public UnityAction OnDead = delegate() { };
        public UnityAction OnBirth = delegate() { };
        public  UnityAction<float> OnChangeHealth=delegate(float f) {  };
    }
}