using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace onlinetutorial.MVVM
{


    public class MainView : MonoBehaviour
    {
        [Header("Ui Elements")] [SerializeField]
        TextMeshProUGUI _PlayerInfoTxt;

        [SerializeField] LeanButton _PlayerInfoBtn;
        [SerializeField] LeanButton _CreateRoomBtn;
        [SerializeField] LeanButton _JoinRoomBtn;


        IMainViewModel _mainViewModel;

        #region UnityMethods

        void Awake()
        {
            _mainViewModel = GetComponent<IMainViewModel>();
        }

        void Start()
        {
           
        }

        void OnEnable()
        {
            _PlayerInfoTxt.SetText(PlayerPrefs.GetString("name"));
            _PlayerInfoBtn.OnClick.AddListener(HandleOnPlayerInfoClicked);
            _CreateRoomBtn.OnClick.AddListener(HandleOnCreateRoomClicked);
            _JoinRoomBtn.OnClick.AddListener(HandleOnJoinRoomClicked);
            UserSignals.Instance.OnChangeName += HandleOnChangeName;

        }

        void OnDisable()
        {

            _PlayerInfoBtn.OnClick.RemoveListener(HandleOnPlayerInfoClicked);
            _CreateRoomBtn.OnClick.RemoveListener(HandleOnCreateRoomClicked);
            _JoinRoomBtn.OnClick.RemoveListener(HandleOnJoinRoomClicked);
            UserSignals.Instance.OnChangeName -= HandleOnChangeName;

        }

        void HandleOnChangeName(string value)
        {
            _PlayerInfoTxt.SetText(_mainViewModel.ChangePlayerName(value));
        }

        void HandleOnPlayerInfoClicked()
        {
            PanelSignals.Instance.OnMenuChange.Invoke(menus.changename);
        }
        
        [ContextMenu("CreateRoom")]
        void HandleOnCreateRoomClicked()
        {
            ButtonSignals.Instance.OnCreateRoom.Invoke();
        }

        void HandleOnJoinRoomClicked()
        {
            ButtonSignals.Instance.OnRoomList.Invoke();
        }

        #endregion

    }
}