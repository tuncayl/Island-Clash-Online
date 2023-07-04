using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace onlinetutorial.data
{
    [Serializable]
    public class AnimationData
    {
        [Header("Grounded Parameter Names")]
        [SerializeField] private string moveParameterName = "move";
        [SerializeField] private string IdleParamterName = "idle";
        [SerializeField] private string JumpingUpParameterName = "jump";
        [SerializeField] private string JumpingDownParameterName = "down";
        [SerializeField] private string FallingParameterName = "fall";
        [FormerlySerializedAs("DieParamterName")] [SerializeField] private string DieParameterName = "dead";
        [SerializeField] private string BirthParameterName = "birth";

        
        public int MovingParameterHash { get; private set; }
        public int IdleParameterHash { get; private set; }

        public int JumpingUpParameterHash { get; private set; }

        public int JumpingDownParameterHash { get; private set; }

        public int FallingParameterHash { get; private set; }
        
        public int DeadParameterHash { get; private set; }
        
        public  int BirthParameterHash { get; private set; }


        public void Initialize()
        {
            IdleParameterHash = Animator.StringToHash(IdleParamterName);
            MovingParameterHash = Animator.StringToHash(moveParameterName);
            JumpingUpParameterHash = Animator.StringToHash(JumpingUpParameterName);
            JumpingDownParameterHash = Animator.StringToHash(JumpingDownParameterName);
            FallingParameterHash = Animator.StringToHash(FallingParameterName);
            DeadParameterHash = Animator.StringToHash(DieParameterName);
            BirthParameterHash = Animator.StringToHash(BirthParameterName);
        }
    }
}