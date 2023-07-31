using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace onlinetutorial.data
{
    [System.Serializable]
    public class MovementData
    {
        //-----MOVEMENT------\\
        [field: Header("Move values")]
        [field: SerializeField]
        public float PMoveSpeed { get; private set; }

        [field: SerializeField] public float SpeedLerp { get; private set; } = 1f;

        [field: SerializeField] public float MoveLerpSmoth { get; private set; } = 15f;

        //-----ROTATION------\\
        [field: Header("Rotation values")]
        [field: SerializeField]
        public float rotationSmoothTime { get; private set; } = 0.15f;

        //----GROUND-----\\
        [field: Header("Ground values")]
        [field: SerializeField]
        public LayerMask groundMask { get; private set; }

        [field: SerializeField] public float radius { get; private set; } = 0.7f;
        [field: SerializeField] public float PlayerHeight { get; private set; } = 0.4f;

        //----jUMP-----\\
        [field: Header("Jump values")]
        [field: SerializeField]
        public float jumpboost { get; private set; } = 6f;

        [field: SerializeField] public float airMultiplier { get; private set; } = 0.4f;
        [field: SerializeField] public float Drag { get; private set; } = 4;

        //----SLOPE-----\\
        [field: Header("Slope Values")]
        [field: SerializeField]
        public float MaxSlopeAngle { get; private set; } = 40f;
    }
}