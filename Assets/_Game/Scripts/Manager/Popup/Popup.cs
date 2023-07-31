using DG.Tweening;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.controllers
{
    public  class Popup: MonoBehaviour
    {
        #region SelfVariables
        private PopupManager popupManager => PopupManager.Instance;
        
        
        
        
        public delegate void ShowEvent(string popupName);
        
        public static event ShowEvent OnShowPopup;
        public static event ShowEvent OnHidePopup;
        public event TweenCallback OnHide;

        #endregion

        #region UnityMethods

        
        public Popup Show(bool createNewAnyway = false,TweenCallback onHide = null)
        {
  
            Popup popup = this;
            popup = GetPopup();
            if (!popup.gameObject.activeSelf) popup.gameObject.SetActive(true);
            popup.name = name;
            popup.OnHide = onHide;
            popup.ShowBeforeAnimation();
            OnShowPopup?.Invoke(popup.name);
            return popup;
        }

        public Popup GetPopup()
        {
            GameObject window = popupManager.transform.Find(name)?.gameObject;
            if (window == null) window = Instantiate(gameObject, popupManager.transform);
            return window.GetComponent<Popup>();
        }
        #endregion

        #region OtherMethods

        protected virtual void ShowBeforeAnimation()
        {
            
        }

        protected virtual void AfterShowAnimation()
        {
            
        }

        public void Hide()
        {
            PanelSignals.Instance.OnChangeRaycaster.Invoke(true);
            OnHidePopup?.Invoke(name);
            HideAnimation();
        }
        private void HideAnimation()
        {
            //setanimation
            
            Destroy(gameObject, 0);
        }
        #endregion



        #region SubscireMethods

        

        #endregion

    }
}