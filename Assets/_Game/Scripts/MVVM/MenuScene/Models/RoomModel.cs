
using UnityEngine;

namespace onlinetutorial.MVVM
{
    [System.Serializable]
    public record RoomModel
    {
        [SerializeField]private GameObject UserPrefab;
        

        public GameObject UserPrefabValue
        {
            get => UserPrefab;
            set => UserPrefab = value;
        }
    }
}