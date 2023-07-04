using System.Collections;
using System.Collections.Generic;
using Keys;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using TMPro;
using UnityEngine;

public class UserProfile : MonoBehaviour, IUilist
{
    #region SelfVariables

    [SerializeField] private TextMeshProUGUI UserText;

    #endregion

    #region InterfaceMethods

    public void SetUiListData(params object[] Data)
    {
        UserText.SetText((Data[0] as UserInfo?).Value.name);
    }

    #endregion
}