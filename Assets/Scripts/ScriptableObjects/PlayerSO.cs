using System.Collections;
using System.Collections.Generic;
using onlinetutorial.Enums;
using UnityEngine;


namespace onlinetutorial.data
{
    [CreateAssetMenu(fileName = "Player", menuName = "Scriptableobjects/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public MovementData movementData { get; private set; }
        [field: SerializeField] public AttackData attackData { get; private set; }

        [field: SerializeField] public HealtData HealtData { get; private set; }
        

    }


    public record ReusableMoveData
    {
        //Move Values
        public Vector2 refmove { get; set; }
        public Vector2 Movelerp { get; set; }
        public Vector3 moveVec = default;
        public float moveSpeed => 10 * speed;
        public float speed { get; set; } = 1f;
        public float RefSpeed { get; set; } = 1f;

        //Rotation Values 
        public float currentAngle { get; set; }

        public float cameraAngle = default;

        //Jump Values 
        public float nextjumprate { get; set; } = 0.7f;
        public bool nextjump => Time.time < nextjumprate;

        //Slope Values
        public RaycastHit SlopeHit;
        
        
    }

    public record ResuableAttackData
    {
        //Attack Values

        public float nextAttackrate { get; set; } = 0.7f;
        public bool nextAttack => Time.time < nextAttackrate;

        public float _currentLayerWeightVelocity = default;
    }

    public record ResuableHealtData
    {
        public ResuableHealtData(float health) => Health = health;
        public float Health { get; set; } = 500f;
    }
    
    public record ResuableGeneralData
    {
        public ResuableGeneralData(chibicolor _chibicolor)
        {
            Chibicolor = _chibicolor;
        }
        public chibicolor Chibicolor;
    }
    
}