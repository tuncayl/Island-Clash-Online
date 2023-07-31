using UnityEngine;

namespace onlinetutorial.data
{
    [System.Serializable]
    public class AttackData
    {
        //Attack general Values
        [field: Header("Attack values")]
        [field: SerializeField]
        public float AttackRate { get; private set; }

        [field: Header("Punch values")]
        [field: SerializeField]
        public float endtime { get; private set; } = 2f;

        public float AttackPower  = 50f;

        [field: Header("Animation Values")]
        [field: SerializeField]
        public float animTransSmoothTime { get; private set; } = 0.15f;
        
        






    }
}