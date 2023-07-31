using System.Collections;
using System.Collections.Generic;
using Keys;
using onlinetutorial.controllers;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using TMPro;
using UnityEngine;

public class RoomProfile : MonoBehaviour, IUilist
{
    #region SelfVaiables

    private int roomid { get; set; } = 0;

    #endregion


    #region Unitymethod

    #endregion


    #region OtherMethods

    public void JoinRoom() => RoomSignals.Instance.OnJoinRoom.Invoke(roomid);

    #endregion

    #region InterfaceMethods

    public void SetUiListData(params object[] Datas)
    {
        RoomData _room = (Datas[0] as RoomData?).Value;
        roomid = _room.Id;
        transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text =
            "ROOM ID : " + _room.Id.ToString() + "  " + _room.Capacity + "/5";
    }

    #endregion
}