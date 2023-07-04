using COMMANDS.Playercommands;
using FishNet;
using FishNet.Component.Observing;
using FishNet.Object;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
namespace onlinetutorial.controllers.Room
{
    public class RoomCounter : SyncCounter, ISetCounter
    {
        #region SelfVariables

        private int? roomid;
        

        #endregion


        #region UnityMethods
        
     


        protected override void Update()
        {
            base.Update();
            UiMenuSignals.Instance.OnChangeCounter.Invoke(minute);
        }

        #endregion


        #region MainMethods
        
        

  
        protected override void OnCompleteClient()
        {
            RoomSignals.Instance.OnSetActiveRoomCounter.Invoke(false);
            SceneSignals.Instance.OnUnLoadScene.Invoke("room");
            SceneSignals.Instance.OnMainSceneloading.Invoke();
        }


        protected override void OnCompleteServer()
        {
            RoomSignals.Instance.OnStartRoom(roomid.Value);
            new UnLoadRoomScene(NetworkManager,   RoomSignals.Instance.OnGetRoomConnections.Invoke(roomid.Value)).execute();
       
        }

        #endregion


        public void SetCounter(int timer, params object[] datas)
        {
            roomid = datas[0] as int?;
            Remaning = timer;
        }
    }
}