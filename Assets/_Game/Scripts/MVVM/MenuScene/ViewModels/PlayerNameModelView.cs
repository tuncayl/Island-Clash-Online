using onlinetutorial.Enums;
using onlinetutorial.Signals;
using TMPro;
using UnityEngine;

namespace onlinetutorial.MVVM
{
    public class PlayerNameModelView : MonoBehaviour,IPlayerNameViewModel
    {
        [SerializeField] private PlayerNameModel _PlayerNameModel;
        
        public void SaveName(TMP_InputField InputField)
        {
            if (InputField.text.Length == 0) return; 
            PlayerPrefs.SetString("name",InputField.text);
            UserSignals.Instance.OnChangeName.Invoke(InputField.text);
            PanelSignals.Instance.OnMenuChange.Invoke(menus.main);
        }
    }
    
    public interface IPlayerNameViewModel
    {
        public void SaveName(TMP_InputField ınputField);
    }
}