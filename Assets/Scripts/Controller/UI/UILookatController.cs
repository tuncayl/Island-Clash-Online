using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using onlinetutorial.Signals;
using Unity.Mathematics;
using UnityEngine;


namespace onlinetutorial.controllers
{
    public class UILookatController : MonoBehaviour
    {
        #region SelfVariables

        private List<Transform> HealtBars = new List<Transform>();

        private Transform OwnerHealtBar;

        private Transform Camera;

 
        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            Subsicre();
        }


        private void OnDisable()
        {
            UnSubsicre();
        }

        private void Start()
        {
            if (InstanceFinder.NetworkManager.IsServer) this.enabled = false;
            Camera = CameraSignals.Instance.OnGetCamera.Invoke().transform;
        }


        private void LateUpdate()
        {
            if (HealtBars.Count == 0) return;
            for (int i = 0; i < HealtBars.Count; i++)
            {
                if (HealtBars[i] == null)
                {
                    HealtBars.RemoveAt(i);
                    continue;
                }
                HealtBars[i].LookAt(Camera);
            }
        }

  

        #endregion

        #region OtherMethods

        private void AddHealtBars(Transform value) => HealtBars.Add(value);

        private void RemoveHealtBars(Transform value) => HealtBars.Remove(value);


        #endregion


        #region SubscireMethods

        private void Subsicre()
        {
            if (InstanceFinder.IsServer) return;
            HealthSignals.Instance.OnHealtBarAdd += AddHealtBars;
            HealthSignals.Instance.OnHealtBarRemove += RemoveHealtBars;

        }

        private void UnSubsicre()
        {
            if (InstanceFinder.IsServer) return;

            HealthSignals.Instance.OnHealtBarAdd -= AddHealtBars;
            HealthSignals.Instance.OnHealtBarRemove -= RemoveHealtBars;

        }

        #endregion
    }
}