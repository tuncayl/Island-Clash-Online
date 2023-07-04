using UnityEngine;

namespace onlinetutorial.MVVM
{
    [System.Serializable]
    public record RoomListModel
    {
        [SerializeField]private GameObject RoomPrefab;
        

        public GameObject RoomPrefabValue
        {
            get => RoomPrefab;
            set => RoomPrefab = value;
        }
    }
}