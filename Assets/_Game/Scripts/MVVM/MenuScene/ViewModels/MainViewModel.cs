using UnityEngine;

namespace onlinetutorial.MVVM
{
    public class MainViewModel : MonoBehaviour,IMainViewModel
    {
        [SerializeField]  private MainModel _model;

        public string PlayerName => _model.PlayerNameValue;
        public string ChangePlayerName(string value)
        {
            _model.PlayerNameValue = value;
            return _model.PlayerNameValue;
        }
    }
    
    public interface IMainViewModel
    {
        string PlayerName { get; }

        string ChangePlayerName(string value);

    }
}