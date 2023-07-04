using System;
using System.Collections.Generic;
using onlinetutorial.abstracts;
using onlinetutorial.Enums;
using onlinetutorial.interfaces;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.controllers
{
    public class MainPanelController : BasePanel
    {
        protected override void Awake()
        {
            base.Awake();
            CreateIMenuEvent();
        }

        public override void CreateIMenuEvent()
        {
            IMenuEvent = new Dictionary<menus, UnityAction>()
            {
                { menus.endgame, () => ButtonSignals.Instance.OnEndGame.Invoke() },
                { menus.startmenu, () => ButtonSignals.Instance.OnMenuLoad.Invoke() }
            };
        }

        public override menus CreateCurrentPanel()
        {
            return menus.countdown;
        }
    }
}