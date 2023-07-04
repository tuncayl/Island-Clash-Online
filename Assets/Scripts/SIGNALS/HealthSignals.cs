using System.Collections;
using System.Collections.Generic;
using onlinetutorial;
using Customdelegates.v2;
using UnityEngine;
using UnityEngine.Events;


namespace onlinetutorial.Signals
{
    public class HealthSignals : MonoSingleton<HealthSignals>
    {
        public UnityAction<Transform> OnHealtBarAdd = delegate(Transform arg0) { };
        public UnityAction<Transform> OnHealtBarRemove = delegate(Transform arg0) { };

        public UnityRefAction<int, float> OnChangeHealth = delegate(ref int arg0, float arg1) { };
    }
}