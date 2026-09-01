using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class HoldPositionCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _detectRadius;

        [SerializeField]
        private float _attackDistance;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData command) == false
                || command is not HoldPositionCommandData commandData
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out AttackComponent attackComponent) == false
                || character.TryGetComponent(out TeamComponent selfTeamComponent) == false
                || _blackboard.TryGetValue(BlackboardAPI.ColliderBuffer, out Collider[] buffer))
                return BehaviourResult.Failure;

            Vector3 selfPosition = character.transform.position;
            
            GameObject target = FindNearestEnemyInRadius(selfPosition, selfTeamComponent.Team, buffer);
            
            if (target != null)
            {
                Vector3 vectorToTarget = target.transform.position - selfPosition;
                vectorToTarget.y = 0f;

                float sqrAttackDistance = _attackDistance * _attackDistance;
                
                if (vectorToTarget.sqrMagnitude <= sqrAttackDistance)
                    attackComponent.Attack(target);
            }
            
            return BehaviourResult.Running;
        }
        
        private GameObject FindNearestEnemyInRadius(Vector3 center, TeamType selfTeam, Collider[] buffer)
        {
            var size = Physics.OverlapSphereNonAlloc(center, _detectRadius, buffer);
            
            GameObject nearestEnemy = null;
            float minSqrDistance = float.MaxValue;

            GameObject nearestTarget = null;

            for (int i = 0; i < size; i++)
            {
                Collider collider = buffer[i];

                if (collider.TryGetComponent(out TeamComponent teamComponent)
                    && selfTeam != teamComponent.Team)
                {
                    float sqrDistance = (collider.transform.position - center).sqrMagnitude;
                    
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestTarget = collider.gameObject;
                    }
                }
            }

            return nearestTarget;
        }
    }
}