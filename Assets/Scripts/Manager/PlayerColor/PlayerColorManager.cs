using System;
using FishNet;
using onlinetutorial.Enums;
using onlinetutorial.Extensions;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.managers
{
    public class PlayerColorManager: MonoBehaviour
    {
        #region SelfVariables

        [SerializeField] private Material[] _ChibiMaterials;
        [SerializeField] private Material[] _ChibiDisolveMaterials;

        #endregion


        #region UnityMethod

        private void Start()
        {
            if(InstanceFinder.IsServer is false)return;
            Array.Clear(_ChibiMaterials,0,_ChibiMaterials.Length);
            Array.Clear(_ChibiDisolveMaterials,0,_ChibiDisolveMaterials.Length);

        }

        private void OnEnable()
        {
            SubscireEvent();
        }

        private void OnDisable()
        {
            UnSubsicreEvent();
        }

        #endregion

        #region OtherMethod

        private (Material,Material) OnGetColor(chibicolor _chibicolor)
        {
            return (_ChibiMaterials[(int)_chibicolor], _ChibiDisolveMaterials[(int)_chibicolor]);
        }

    
        #endregion


        #region SubscireEvent

        private void SubscireEvent()
        {
            if(InstanceFinder.IsServer)return;
            PlayerSignal.Instance.OnGetColor += OnGetColor;
        }

        private void UnSubsicreEvent()
        {
            if(InstanceFinder.IsServer)return;
            PlayerSignal.Instance.OnGetColor -= OnGetColor;

        }

        #endregion
    }
}