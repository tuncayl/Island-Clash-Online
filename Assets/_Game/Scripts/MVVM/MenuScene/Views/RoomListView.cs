using System.Collections.Generic;
using Keys;
using Lean.Gui;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.MVVM
{
    public class RoomListView: MonoBehaviour
    {
        [Header("Ui Elements")] 
        [SerializeField] private Transform _RoomListRoot;
        [SerializeField] LeanButton _BackBtn;



         IRoomListViewModel _roomListViewModel;

        #region UnityMethods

        void Awake()
        {
            _roomListViewModel = GetComponent<IRoomListViewModel>();
        }

        void Start()
        {
        }

        void OnEnable()
        {
            _BackBtn.OnClick.AddListener(HandleOnBackBtnClicked);
            UiMenuSignals.Instance.OnUiListedRoom += HandleOnListRooms;


        }

        void OnDisable()
        {

            _BackBtn.OnClick.RemoveListener(HandleOnBackBtnClicked);
            
        }

        void HandleOnBackBtnClicked()
        {
            PanelSignals.Instance.OnMenuChange.Invoke(menus.main);
        }

        void HandleOnListRooms(ref List<RoomData> datas)
        {
            _roomListViewModel.RoomSetPlayers(ref datas,ref _RoomListRoot);
        }

      
        #endregion
    }
}