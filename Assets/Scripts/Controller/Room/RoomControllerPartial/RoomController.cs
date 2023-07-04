using System;
using System.Collections.Generic;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using Keys;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace onlinetutorial.controllers
{
    public partial class RoomController : NetworkBehaviour
    {
        #region SelfVariables

        private int roomIdCounter;

        private int roomIdCounterProperty
        {
            get { return roomIdCounter; }
            set
            {
                if (roomIdCounter == 9999) roomIdCounter = 0;
                else roomIdCounter = value;
            }
        }

        private Dictionary<int, room> Rooms;

        private int currentRoomid;

        private bool Host = false;

        #endregion

        #region UnityMethods

        public override void OnStartServer()
        {
            base.OnStartServer();
            Rooms = new Dictionary<int, room>();
        }

        private void Start()
        {
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        private void OnEnable()
        {
            Subscire();
        }

        private void OnDisable()
        {
            UnSubscire();
        }

        #endregion

        #region MainMethods

        private void OnCreateRoom() => CreateRoom(UserSignals.Instance.OnGetconnection.Invoke());


        private void OnRoomBack() => ExitRoom(UserSignals.Instance.OnGetconnection.Invoke(), currentRoomid);

        private void OnJoinRoom(int _roomid) => JoinRoom(UserSignals.Instance.OnGetconnection.Invoke(), _roomid);

        private void OnStartCount() => StartCount(currentRoomid);

        private void OnStartRoom(int roomid) => StartRoom(roomid);

        private void OnRoomList()
        {
            PanelSignals.Instance.OnMenuChange.Invoke(menus.roomlist);
            ListRooms(UserSignals.Instance.OnGetconnection.Invoke());
        }

        //RefleshRoom-----------------------
        private void RefleshRoom()
        {
            List<UserInfo> userDatas = new List<UserInfo>();
            UserSignals.Instance.OnRoomAdduser.Invoke(ref userDatas);
            UiMenuSignals.Instance.OnUiListedPlayer.Invoke(ref userDatas);
        }

        private int OnGetRoomid() => currentRoomid;

        #endregion

        #region OtherMethods

        private void OnDisconectedUser(int roomid, NetworkConnection con)
        {
            if (Rooms.ContainsKey(roomid) is false) return;
            DisconnectedRoom(con, roomid);
        }

        private List<NetworkConnection> OnGetRoomConnections(int roomid) => Rooms[roomid].Players;


        private void OnSetRoomScene(ref Scene RoomScene,int roomid)
        {
            if(Rooms.ContainsKey(roomid) is false)return;
            Rooms[roomid].RoomScene = RoomScene;
            GameObject spawnobject=Instantiate(Resources.Load("SyncRoomTimer", typeof(GameObject))) as GameObject;
            spawnobject.transform.parent = Rooms[roomid].RoomScene.GetRootGameObjects()[0].transform;
            if (spawnobject.TryGetComponent<ISetCounter>(out ISetCounter counter))counter.SetCounter(4,roomid);
            Spawn(spawnobject);
        }

        #endregion

        #region ServerMethods


        #endregion


        #region SubscireMethods

        private void Subscire()
        {
            UserSignals.Instance.OnDisconnected += OnDisconectedUser;
            
            ButtonSignals.Instance.OnCreateRoom += OnCreateRoom;
            ButtonSignals.Instance.OnRoomBack += OnRoomBack;
            ButtonSignals.Instance.OnRoomList += OnRoomList;
            ButtonSignals.Instance.OnStartCount += OnStartCount;

            RoomSignals.Instance.OnGetRoomid += OnGetRoomid;
            RoomSignals.Instance.OnJoinRoom += OnJoinRoom;
            RoomSignals.Instance.OnStartRoom += OnStartRoom;
            RoomSignals.Instance.OnGetRoomConnections += OnGetRoomConnections;
            RoomSignals.Instance.OnClearRoom += OnClearRoom;

            SceneSignals.Instance.OnRoomSceneloaded += OnSetRoomScene;
        }

        private void UnSubscire()
        {
            UserSignals.Instance.OnDisconnected -= OnDisconectedUser;


            ButtonSignals.Instance.OnCreateRoom -= OnCreateRoom;
            ButtonSignals.Instance.OnRoomBack -= OnRoomBack;
            ButtonSignals.Instance.OnRoomList -= OnRoomList;
            ButtonSignals.Instance.OnStartCount -= OnStartCount;
            RoomSignals.Instance.OnClearRoom -= OnClearRoom;

            RoomSignals.Instance.OnGetRoomid -= OnGetRoomid;
            RoomSignals.Instance.OnJoinRoom -= OnJoinRoom;
            RoomSignals.Instance.OnStartRoom -= OnStartRoom;
            RoomSignals.Instance.OnGetRoomConnections -= OnGetRoomConnections;
            
            SceneSignals.Instance.OnRoomSceneloaded -= OnSetRoomScene;

        }

        #endregion
    }

    
}