using COMMANDS.Playercommands;
using Lean.Gui;
using onlinetutorial.Enums;
using onlinetutorial.Signals;
using UnityEngine;

namespace onlinetutorial.MVVM
{
    public class EndGameView : MonoBehaviour
    {
        #region SelfVariables

        [Header("UIObjects")] 
        [SerializeField] private Transform RootUser;

        [SerializeField] private LeanButton LoadMainBtn;
        #endregion

        #region UnityMethods

        void Awake()
        {
            
        }

        void Start()
        {
        }

        void OnEnable()
        {
            LoadMainBtn.OnClick.AddListener(HandleOnLoadMainBtn);
        }

        void OnDisable()
        {
            LoadMainBtn.OnClick.RemoveListener(HandleOnLoadMainBtn);

        }

        #endregion

        private void HandleOnLoadMainBtn()
        {
            Debug.Log("HANDLE LOAD");
            PanelSignals.Instance.OnButtonClicked.Invoke(menus.startmenu,false);
        }
        
    }
}