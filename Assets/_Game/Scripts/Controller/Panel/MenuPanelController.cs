using System.Collections.Generic;
using onlinetutorial.abstracts;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.controllers
{
    public class MenuPanelController : BasePanel
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
                { menus.startmain ,ButtonSignals.Instance.OnStartCount}
            };
        }

        public override menus CreateCurrentPanel()
        {
            return menus.main;
        }
    }
}