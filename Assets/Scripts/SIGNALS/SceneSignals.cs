using System;
using Customdelegates.v1;
using Keys;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace onlinetutorial.Signals
{
    public class SceneSignals: MonoSingleton<SceneSignals>
    {
        protected void Start()
        {
            DontDestroyOnLoad(this);

        }
        //MainScene Load
        public  UnityAction<Sceneinfo> OnMainSceneload=delegate(Sceneinfo roomid) {  };
        public  UnityAction OnMainSceneloading=delegate (){  };
        public UnityAction OnMainSceneloaded=delegate (){  };
        public  Func<Sceneinfo> OnGetMainSceneInfo= delegate() {return  default;};
        
        
        //RoomScene load
        public  UnityRefAction<int> OnRoomSceneload=delegate(ref int roomid) {  };
        public  Customdelegates.v2.UnityRefAction<Scene,int> OnRoomSceneloaded=delegate (ref Scene roominfo,int roomid){  };
        
        
        //Menu load
        public UnityAction OnMenuSceneLoading=delegate {  };
        public UnityAction OnMenuSceneLoaded =delegate {  };


        //general UnLoad Que
        public  UnityAction<string> OnUnLoadScene=delegate(string arg0) {  };


    }
}