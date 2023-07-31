using UnityEngine;

namespace onlinetutorial.MVVM
{
    [System.Serializable]
    public record MainModel
    {
        [SerializeField] string _PlayerName = "Tuncayll";
        
        public string PlayerNameValue
        {
            get => _PlayerName;
            set => _PlayerName = value;
        }
    }
}