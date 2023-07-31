using System;
using Cinemachine;
using FishNet;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace onlinetutorial.controllers
{
    public class CameraManager : MonoBehaviour
    {
        #region SelfVariables

        [FormerlySerializedAs("cam")] [Header("Camera")] [SerializeField] Camera _cam;
        [Header("VmCamera")] [SerializeField] private CinemachineFreeLook _virtualCamera;
        
        [Header("CursorSetting")]
        [SerializeField] private bool _visiblecursor;
        [SerializeField] private CursorLockMode _lockMode;

        #endregion


        #region UnityMethod

        private void Awake()
        {
            INIT();
        }

        private void Start()
        {
            if (InstanceFinder.IsServer)return;
            _cam.gameObject.GetComponent<AudioListener>().enabled = true;
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

        private void INIT()
        {
            //Cursor Setting
            Cursor.lockState = _lockMode;
            Cursor.visible = _visiblecursor;
        }

        

        private Camera GetCamera() => _cam;
        private Transform GetVmCamera () => _virtualCamera.transform;


        private void OnSetVmCamera(Transform target)
        {
            _virtualCamera.Follow = target;
            _virtualCamera.LookAt = target;
        }

        private void OnGameEnd()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _virtualCamera.enabled = false;
        }
        #endregion

        #region SubscireEvent

        private void SubscireEvent()
        {
            CoreGameSignals.Instance.OnGameEnd += OnGameEnd;
            CameraSignals.Instance.OnGetCamera += GetCamera;
            CameraSignals.Instance.OnGetVmCamera += GetVmCamera;
            CameraSignals.Instance.OnSetVmCamera += OnSetVmCamera;
        }

        private void UnSubsicreEvent()
        {
            CoreGameSignals.Instance.OnGameEnd -= OnGameEnd;
            CameraSignals.Instance.OnGetCamera -= GetCamera;
            CameraSignals.Instance.OnGetVmCamera -= GetVmCamera;
            CameraSignals.Instance.OnSetVmCamera -= OnSetVmCamera;

        }

        #endregion
    }
}