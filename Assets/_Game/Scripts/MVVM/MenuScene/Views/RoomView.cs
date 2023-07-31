using System.Collections;
using System.Collections.Generic;
using Keys;
using Lean.Gui;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace onlinetutorial.MVVM
{
    public class RoomView : MonoBehaviour
    {
        [FormerlySerializedAs("_StartButton")] [Header("Ui Elements")] [SerializeField]
        LeanButton _StartRoomBtn;
        [SerializeField] LeanButton _BackBtn;

        IRoomViewModel _mainViewModel;

        [SerializeField] private Transform UserRoot;

        [SerializeField] private TextMeshProUGUI UiCountertext;
        [SerializeField] private TextMeshProUGUI UiRoomId;
        #region UnityMethods

        void Awake()
        {
            _mainViewModel = GetComponent<IRoomViewModel>();
        }

        void Start()
        {
        }

        void OnEnable()
        {
            _StartRoomBtn.OnClick.AddListener(HandleOnStartRoomClicked);
            _BackBtn.OnClick.AddListener(HandleOnBackClicked);
            UiMenuSignals.Instance.OnUiListedPlayer += HandleOnListPlayers;
            RoomSignals.Instance.OnChangeHost += HandleOnChangeHost;
            RoomSignals.Instance.OnSetActiveRoomCounter += HandleOnStartCounter;
            RoomSignals.Instance.OnCreatedRoom += HandleOnCreatedRoom;
            UiMenuSignals.Instance.OnChangeCounter += HandleOnChageCounter;



        }

        void OnDisable()
        {
            _StartRoomBtn.OnClick.RemoveListener(HandleOnStartRoomClicked);
            _BackBtn.OnClick.RemoveListener(HandleOnBackClicked);
            UiMenuSignals.Instance.OnUiListedPlayer -= HandleOnListPlayers;
            RoomSignals.Instance.OnChangeHost -= HandleOnChangeHost;
            RoomSignals.Instance.OnCreatedRoom -= HandleOnCreatedRoom;
            RoomSignals.Instance.OnSetActiveRoomCounter -= HandleOnStartCounter;

        }

        void HandleOnStartRoomClicked()
        {
            PanelSignals.Instance.OnButtonClicked.Invoke(menus.startmain,false);
        }

        void HandleOnBackClicked()
        {
            ButtonSignals.Instance.OnRoomBack.Invoke();
        }

        void HandleOnListPlayers(ref List<UserInfo>  value)
        {
            _mainViewModel.RoomSetPlayers(ref value,ref UserRoot);
        }

        void HandleOnChangeHost(bool value)
        {
            _StartRoomBtn.gameObject.SetActive(value);
        }
        
        void HandleOnStartCounter(bool value)
        {
            _StartRoomBtn.gameObject.SetActive(false);
            UiCountertext.gameObject.SetActive(value);
        }
        
        void HandleOnChageCounter(int value)
        {
            UiCountertext.SetText(value.ToString());
        }

        void HandleOnCreatedRoom(int roomid)
        {
            UiRoomId.SetText(roomid.ToString());
        }
        #endregion
    }
}