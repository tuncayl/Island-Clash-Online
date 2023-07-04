

using onlinetutorial.interfaces;
using onlinetutorial.controllers;
using UnityEngine;

namespace COMMANDS.Playercommands
{
    public class AttackCommand:PlayerCommand
    {
        private PlayerPhysicsController playerPhysicsController;

        private Vector3 attackposition;

        private float attackpower;
        public AttackCommand(PlayerPhysicsController _playerPhsicsController,Vector3 _attackposition,ref  float _attackpower)
        {
            playerPhysicsController = _playerPhsicsController;
            attackposition = _attackposition;
            attackpower = _attackpower;
        }
        
        
        public override void execute()
        {
            Physics.SyncTransforms();
            Collider[] colliders = new Collider[5];
            int found = playerPhysicsController.physicsScene.OverlapSphere(attackposition,
                0.85f, colliders,
                playerPhysicsController._Playermask,
                QueryTriggerInteraction.Collide);
      
            if (colliders[0] == null)
            {
                Debug.Log("not found colliders" + found);
                return;
            }
            Debug.Log("founded colliders" + found);
            if (colliders[0].TryGetComponent<IHealth>(out IHealth health))
                health.TakeDamage(attackpower);
        }
    }
}