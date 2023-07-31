using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace onlinetutorial.Signals
{
    public class CameraSignals : MonoSingleton<CameraSignals>
    {
        public Func<Camera> OnGetCamera = delegate() { return default; };
        public  Func<Transform> OnGetVmCamera= delegate() { return default;};
        public UnityAction<Transform> OnSetVmCamera=delegate (Transform arg0){  };
    }
}

