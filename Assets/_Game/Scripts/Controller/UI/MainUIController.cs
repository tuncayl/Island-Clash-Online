using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    #region SelfVariables

    [Header("EventSystem")] 
    [SerializeField]private EventSystem SceneEventSystem;

    [SerializeField] private GraphicRaycaster _raycaster;
  
    #endregion

    #region UnityMethods

    private void Start()
    {
        if(InstanceFinder.IsServer)return;
        SceneEventSystem.enabled = true;
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



    private void OnGameEnd()
    {
        PanelSignals.Instance.OnMenuChange.Invoke(menus.endgame);
    }
 
    private void OnChangeRaycaster(bool value)
    {
        _raycaster.enabled = value;
    }
    #endregion

    #region OtherMethods

    

    #endregion

    #region SubscireMethods

    private void Subscire()
    {
       if(InstanceFinder.IsServer)return;
       CoreGameSignals.Instance.OnGameEnd += OnGameEnd;
       PanelSignals.Instance.OnChangeRaycaster += OnChangeRaycaster;

    }

   

    private void UnSubscire()
    {
        if(InstanceFinder.IsServer)return;
        CoreGameSignals.Instance.OnGameEnd += OnGameEnd;
        PanelSignals.Instance.OnChangeRaycaster -= OnChangeRaycaster;

    }

    #endregion
}