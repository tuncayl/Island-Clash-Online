using System;
using System.Collections;
using System.Collections.Generic;
using onlinetutorial.controllers;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    #region SelfVariables

    [Header("Referances")] 
    private MenuPanelController MenuPanelController;

    [Header("GENERAL/Raycaster")]
    [SerializeField] private GraphicRaycaster UiRaycaster;
    

    #endregion

    #region UnityMethods

    private void Awake()
    {
        MenuPanelController = GetComponent<MenuPanelController>();
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

    #region OtherMethods



    private void OnConnectedToServer()
    {
        Camera.main.GetComponent<AudioListener>().enabled = false;
        EventSystem.current.enabled = false;
    }


    private void OnChangeRaycaster(bool value) => UiRaycaster.enabled = value;


    private void OnMainSceneLoading()
    {
        PanelSignals.Instance.OnCloseCurrentPanel.Invoke();
        PanelSignals.Instance.OnChangeRaycaster.Invoke(true);
        MenuPanelController.enabled = false;
    }

    private void OnMenuSceneLoaded()
    {
        OnChangeRaycaster(true);
        MenuPanelController.enabled = true;
        PanelSignals.Instance.OnMenuChange.Invoke(menus.main);
    }
    
    #endregion

    #region ReusableMethods
    

    #endregion

    #region SubscireMethods

    private void Subscire()
    {
        SceneSignals.Instance.OnMainSceneloading += OnMainSceneLoading;
        SceneSignals.Instance.OnMenuSceneLoaded += OnMenuSceneLoaded;
        PanelSignals.Instance.OnChangeRaycaster += OnChangeRaycaster;
        CoreGameSignals.Instance.OnConnectedServer += OnConnectedToServer;
    }

    private void UnSubscire()
    {
        SceneSignals.Instance.OnMenuSceneLoaded -= OnMenuSceneLoaded;
        SceneSignals.Instance.OnMainSceneloading -= OnMainSceneLoading;
        PanelSignals.Instance.OnChangeRaycaster -= OnChangeRaycaster;
        CoreGameSignals.Instance.OnConnectedServer -= OnConnectedToServer;
    }

    #endregion
}