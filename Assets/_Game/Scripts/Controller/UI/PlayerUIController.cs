using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using onlinetutorial.controllers;
using onlinetutorial.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUIController : NetworkBehaviour
{
    #region SelfVariables

    private Player _player { get; set; }

    [Header("UIObjects/Root")] [SerializeField]
    private GameObject AllPanels;

    [Header("UIObjects/Users")] [SerializeField]
    private Transform UserPanel;

    [SerializeField] private TextMeshProUGUI UserText;

    [FormerlySerializedAs("HealthCanvas")] [Header("UIObjects/Healths")] [SerializeField]
    private Transform HealthPanel;

    [SerializeField] private Image HealthBar;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<Player>();
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

    #region ServerMethods

    public override void OnStartClient()
    {
        base.OnStartClient();
        HealthSignals.Instance.OnHealtBarAdd?.Invoke(AllPanels.transform);
        if (IsOwner is false) return;
        SendServerChangeName(UserSignals.Instance.OnGetUserData.Invoke().name);
    }

    [ServerRpc(RequireOwnership = true)]
    private void SendServerChangeName(string value) => SendObserverChangeName(value);

    [ObserversRpc(BufferLast = true)]
    private void SendObserverChangeName(string value) => OnChangeName(value);

    #endregion


    #region mainmethods

    private void OnChangeName(string value) => UserText.text = value;
    private void OnChangeHealth(float value) => HealthBar.fillAmount = value;

    private void OnPlayerBirth() => AllPanels.gameObject.SetActive(true);

    private void OnPlayerDead() => AllPanels.gameObject.SetActive(false);

    #endregion


    #region SubscireMethods

    private void Subscire()
    {
        //Client side
        _player._PlayerLocalSignals.OnBirth += OnPlayerBirth;
        _player._PlayerLocalSignals.OnChangeHealth += OnChangeHealth;
        _player._PlayerLocalSignals.OnDead += OnPlayerDead;
        if (_player.IsOwner is false) return;
        //owner side
    }

    private void UnSubscire()
    {
        //Client side
        _player._PlayerLocalSignals.OnBirth -= OnPlayerBirth;
        _player._PlayerLocalSignals.OnChangeHealth -= OnChangeHealth;
        _player._PlayerLocalSignals.OnDead -= OnPlayerDead;
        if (_player.IsOwner is false) return;
        //owner side
    }

    #endregion
}