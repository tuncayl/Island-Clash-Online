using System.Collections;
using System.Collections.Generic;
using onlinetutorial.Signals;
using UnityEngine;


namespace onlinetutorial.controllers
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        #region SelfVariables

        public Popup info;
    
        private string lastPopup;

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

        #endregion

        #region OtherMethods
        
        
        public void Show(string menuName)
        {
            if (GetAttribute<Popup>(menuName) == null)
            {
                Debug.LogWarning("popup name is null !!");
                return;
            }
            GetAttribute<Popup>(menuName).Show();
        }

        public T GetAttribute<T>(string _name)
        {
            return (T) typeof(PopupManager).GetField(_name).GetValue(this);
        }

        private void OnShowPopup(string popupname)
        {
            lastPopup = popupname;
        }

        #endregion


        #region SubscireMethods

        private void Subscire()
        {
            Popup.OnShowPopup += OnShowPopup;

        }

        private void UnSubscire()
        {
            Popup.OnShowPopup -= OnShowPopup;

        }
        #endregion
    }
}