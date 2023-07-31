using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using Keys;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;


public class SceneLoadController : MonoSingleton<SceneLoadController>
{
    #region SelfVariables

    private Queue<Sceneinfo> loadingMainScenes = new Queue<Sceneinfo>();
    private Queue<int> loadingRoomScenes = new Queue<int>();

    private Queue<string> UnLoadScenes = new Queue<string>();

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        Subscire();
    }

    private void OnDisable()
    {
        UnSubscire();
    }

    protected void Start()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region MainMethods

    private void OnLoadEndServer(SceneLoadEndEventArgs startEventArgs)
    {
        if (!InstanceFinder.IsServer) return;
        for (int i = 0; i < startEventArgs.LoadedScenes.Length; i++)
        {
            Debug.Log(startEventArgs.LoadedScenes[i].name + " ON LOAD END SERVER");
            if (startEventArgs.LoadedScenes[i].name == "room")
                SceneSignals.Instance.OnRoomSceneloaded.Invoke(ref startEventArgs.LoadedScenes[i],
                    loadingRoomScenes.Dequeue());
            if (startEventArgs.LoadedScenes[i].name == "menu")
                SceneSignals.Instance.OnMenuSceneLoaded.Invoke();
        }
    }

    private void OnLoadEndClient(SceneLoadEndEventArgs startEventArgs)
    {
        if (InstanceFinder.IsServer) return;
        for (int i = 0; i < startEventArgs.LoadedScenes.Length; i++)
        {
            Debug.Log(startEventArgs.LoadedScenes[i].name + " ON LOAD END CLIENT");

            if (startEventArgs.LoadedScenes[i].name == "menu")
                SceneSignals.Instance.OnMenuSceneLoaded.Invoke();
        }
    }

    private void OnUnLoadEndClient(SceneUnloadEndEventArgs startEventArgs)
    {
        if (InstanceFinder.IsServer) return;

//       if (UnLoadScenes.Dequeue() == "main") SceneSignals.Instance.OnMainSceneUnLoaded.Invoke();
    }

    #endregion

    #region ServerRpc

    #endregion

    #region OtherMethods

    private void OnBeforeRoomSceneLoad(ref int id) => loadingRoomScenes.Enqueue(id);

    private void AddRoomidQue(Sceneinfo _sceneinfo) => loadingMainScenes.Enqueue(_sceneinfo);

    private Sceneinfo OnGetSceneInfo() => loadingMainScenes.Dequeue();

    private void OnMainSceneLoaded() => UnitySceneManager.SetActiveScene(UnitySceneManager.GetSceneByName("main"));
    //private void OnMenuSceneLoaded() => UnitySceneManager.SetActiveScene(UnitySceneManager.GetSceneByName("menu"));

    private void OnUnLoadScene(string scene) => UnLoadScenes.Enqueue(scene);


    private void OnMenuSceneLoading() => AsyncMenuSceneLoading();


    private async UniTask AsyncMenuSceneLoading()
    {
        await UniTask.WaitUntil(() => UnitySceneManager.GetActiveScene().name == "menu");
        SceneSignals.Instance.OnMenuSceneLoaded.Invoke();
    }

    #endregion

    #region SubscireMethods

    private void Subscire()
    {
        SceneSignals.Instance.OnMainSceneload += AddRoomidQue;
        SceneSignals.Instance.OnGetMainSceneInfo += OnGetSceneInfo;
        SceneSignals.Instance.OnMainSceneloaded += OnMainSceneLoaded;

        SceneSignals.Instance.OnRoomSceneload += OnBeforeRoomSceneLoad;
        //SceneSignals.Instance.OnMenuSceneLoaded += OnMenuSceneLoaded;

        SceneSignals.Instance.OnUnLoadScene += OnUnLoadScene;

        SceneSignals.Instance.OnMenuSceneLoading += OnMenuSceneLoading;


        if (InstanceFinder.SceneManager is null) return;
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadEndServer;
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadEndClient;
        InstanceFinder.SceneManager.OnUnloadEnd += OnUnLoadEndClient;
    }


    private void UnSubscire()
    {
        SceneSignals.Instance.OnMainSceneloaded -= OnMainSceneLoaded;
        SceneSignals.Instance.OnMainSceneload -= AddRoomidQue;
        SceneSignals.Instance.OnGetMainSceneInfo -= OnGetSceneInfo;

        SceneSignals.Instance.OnRoomSceneload -= OnBeforeRoomSceneLoad;
        //SceneSignals.Instance.OnMenuSceneLoaded -= OnMenuSceneLoaded;

        SceneSignals.Instance.OnUnLoadScene -= OnUnLoadScene;

        SceneSignals.Instance.OnMenuSceneLoading -= OnMenuSceneLoading;

        if (InstanceFinder.SceneManager is null) return;
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadEndServer;
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadEndClient;
        InstanceFinder.SceneManager.OnUnloadEnd -= OnUnLoadEndClient;
    }

    #endregion
}
