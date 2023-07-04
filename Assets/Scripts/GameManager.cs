using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    #region SelfVariables

    

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
    private void Awake()
    {
        Application.targetFrameRate = 144;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region OtherMethods

 

    #endregion


    #region SubscireMethods

    private void Subscire()
    {
        
    }

    private void UnSubscire()
    {
        
    }
    #endregion
}
