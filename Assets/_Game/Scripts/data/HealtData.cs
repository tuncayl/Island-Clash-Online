using UnityEngine;

namespace onlinetutorial.data
{
    [System.Serializable]
    public class HealtData
    {
        [field: Header("GENERAL DATAS")]
        [field: SerializeField] 
        public float MaxHealth { get; private set; }

        
    }
}