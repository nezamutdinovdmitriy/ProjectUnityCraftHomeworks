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
                || character.TryGetComponent(out TeamComponent selfTeamComponent) == false)
                return BehaviourResult.Failure;

            Vector3 selfPosition = character.transform.position;
            
            GameObject target = FindNearestEnemyInRadius(selfPosition, selfTeamComponent.Team);
            
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
        
        private GameObject FindNearestEnemyInRadius(Vector3 center, TeamType selfTeam)
        {
            Collider[] colliders = Physics.OverlapSphere(center, _detectRadius);
            GameObject nearestEnemy = null;
            float minSqrDistance = float.MaxValue;

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out TeamComponent team) && team.Team != selfTeam)
                {
                    float sqrDist = (col.transform.position - center).sqrMagnitude;
                    if (sqrDist < minSqrDistance)
                    {
                        minSqrDistance = sqrDist;
                        nearestEnemy = col.gameObject;
                    }
                }
            }

            return nearestEnemy;
        }
    }
}