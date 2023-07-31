using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FishNet;
using FishNet.Managing.Client;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using FishNet.Managing;
using onlinetutorial.Signals;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoSingleton<LoadingController>
{
    #region SelfVariables

    [SerializeField] private GameObject LoadingParent = default;
    [SerializeField] private DOTweenAnimation fadeAnimation = default;

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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region OtherMethods

    private void LoadingComplete()
    {

        LoadingParent.SetActive(false);
        fadeAnimation.DORestart();
    }

    private void LoadingStart()
    {
        LoadingParent.SetActive(true);
        fadeAnimation.transform.GetComponent<Image>().enabled = true;
        fadeAnimation.transform.GetComponent<Image>().color=Color.black;
    }
    
    
    
    #endregion


    #region SubscireMethods

    private void Subscire()
    {
        SceneSignals.Instance.OnMainSceneloading += LoadingStart;
        SceneSignals.Instance.OnMenuSceneLoading += LoadingStart;
        
        SceneSignals.Instance.OnMainSceneloaded += LoadingComplete;
        SceneSignals.Instance.OnMenuSceneLoaded += LoadingComplete;
        
        CoreGameSignals.Instance.OnConnectedClient += LoadingComplete;

    }

    private void UnSubscire()
    {
        SceneSignals.Instance.OnMainSceneloading -= LoadingStart;
        SceneSignals.Instance.OnMenuSceneLoading -= LoadingStart;
        
        SceneSignals.Instance.OnMainSceneloaded -= LoadingComplete;
        SceneSignals.Instance.OnMenuSceneLoaded -= LoadingComplete;

        CoreGameSignals.Instance.OnConnectedClient -= LoadingComplete;

    }

    #endregion
}