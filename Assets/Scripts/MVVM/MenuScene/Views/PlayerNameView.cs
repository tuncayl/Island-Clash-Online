using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace onlinetutorial.MVVM
{
    public class PlayerNameView : MonoBehaviour
    {
        [Header("Ui Elements")] [SerializeField]
        LeanButton _SaveButton;

        [SerializeField] private TMP_InputField _InputField; 

        IPlayerNameViewModel _playerNameViewModel;
        

        #region UnityMethods

        void Awake()
        {
            _playerNameViewModel = GetComponent<IPlayerNameViewModel>();
        }

        void Start()
        {
        }

        void OnEnable()
        {
            _InputField.text = PlayerPrefs.GetString("name","DEFAULT");
            
            _SaveButton.OnClick.AddListener(HandleOnSaveBtnClicked);
            
        }

        void OnDisable()
        {
            _SaveButton.OnClick.RemoveListener(HandleOnSaveBtnClicked);


        }

        void HandleOnSaveBtnClicked()
        {
            _playerNameViewModel.SaveName(_InputField);
        }

       

       

        #endregion
    }
}