using System;
using System.Collections.Generic;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace onlinetutorial.abstracts
{
    public abstract class BasePanel : MonoBehaviour
    {
        #region SelfVariables

        [field:SerializeField] private List<panels> _panelsList { get;  set; }


        private Dictionary<menus, panels> Ipanels = new Dictionary<menus, panels>();
        protected Dictionary<menus, UnityAction> IMenuEvent;

        private menus currentpanel = menus.main;

        #endregion

        #region UnityMethods

        protected virtual   void Awake()
        {
            foreach (menus i in Enum.GetValues(typeof(menus)))
            {
                Ipanels.Add(i, _panelsList.Find(x => x.menu == i));
            }
            currentpanel= CreateCurrentPanel();

        }

        protected void Start()
        {
          
        }

        protected virtual void OnEnable()
        {
            Subscire();
        }

        protected virtual void OnDisable()
        {
            UnSubscire();
        }

        #endregion

        #region MainMethods

        public abstract void CreateIMenuEvent();

        public abstract menus CreateCurrentPanel();

        protected void OnMenuChange(menus _menu)
        {
            if (Ipanels[_menu].activeobject is null) return;
            Ipanels[currentpanel].activeobject.SetActive(false);
            Ipanels[_menu].activeobject.SetActive(true);
            currentpanel = _menu;
            PanelSignals.Instance.OnChangeRaycaster.Invoke(true);
        }

        protected void OnButtonClicked(menus _menu, bool raycaster)
        {
            if (IMenuEvent.ContainsKey(_menu) is false) return;
            IMenuEvent[_menu].Invoke();
            PanelSignals.Instance.OnChangeRaycaster.Invoke(raycaster);
        }

        protected void OnCloseCurrentPanel()
        {
            if (Ipanels[currentpanel].activeobject is null) return;
            Ipanels[currentpanel].activeobject.SetActive(false);

        }

        private void test()
        {
            Debug.Log("TESTTING");
        }

        #endregion

        #region SubscireMethods

        private void Subscire()
        {
            PanelSignals.Instance.OnMenuChange += OnMenuChange;
            PanelSignals.Instance.OnButtonClicked += OnButtonClicked;
            PanelSignals.Instance.OnCloseCurrentPanel += OnCloseCurrentPanel;
        }

        private void UnSubscire()
        {
            PanelSignals.Instance.OnMenuChange -= OnMenuChange;
            PanelSignals.Instance.OnButtonClicked -= OnButtonClicked;
            PanelSignals.Instance.OnCloseCurrentPanel -= OnCloseCurrentPanel;

        }

        #endregion
    }

    [System.Serializable]
    public record panels
    {
        public menus menu;
        public GameObject activeobject;
    }
}