using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AttackCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _attackDistance;

        [SerializeField]
        private float _detectRadius;

        [SerializeField]
        private float _stoppingDistance;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData) == false
                || commandData is not AttackCommandData attackCommandData
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out MoveComponent moveComponent) == false
                || character.TryGetComponent(out AttackComponent attackComponent) == false
                || character.TryGetComponent(out TeamComponent teamComponent) == false)
                return BehaviourResult.Failure;
            
            Vector3 selfPosition = character.transform.position;
            
            if (attackCommandData.Point.Target != null)
            {
                GameObject target = attackCommandData.Point.Target;
                if (target.TryGetComponent(out TeamComponent enemyTeam) == false 
                    || enemyTeam.Team == teamComponent.Team)
                {
                    ResetCommand();
                    return BehaviourResult.Failure;
                }

                return ProcessTargetAttack(character, target, moveComponent, attackComponent, deltaTime);
            }

            if (attackCommandData.Point.Position.HasValue)
            {
                Vector3 targetPoint = attackCommandData.Point.Position.Value;
                
                GameObject visibleEnemy = FindNearestEnemyInRadius(selfPosition, teamComponent.Team);

                if (visibleEnemy != null)
                    return ProcessTargetAttack(character, visibleEnemy, moveComponent, attackComponent, deltaTime);
                
                Vector3 moveVector = targetPoint - selfPosition;
                moveVector.y = 0f;

                if (moveVector.sqrMagnitude <= _stoppingDistance * _stoppingDistance)
                {
                    ResetCommand();
                    return BehaviourResult.Success;
                }

                moveComponent.MoveStep(moveVector.normalized, deltaTime);
                return BehaviourResult.Running;
            }
            
            ResetCommand();
            return BehaviourResult.Failure;
        }

        private BehaviourResult ProcessTargetAttack(
            GameObject character,
            GameObject enemy,
            MoveComponent moveComponent,
            AttackComponent attackComponent,
            float deltaTime)
        {
            Vector3 selfPosition = character.transform.position;
            Vector3 targetPosition = enemy.transform.position;

            Vector3 vector = targetPosition - selfPosition;
            vector.y = 0f;

            float sqrAttackDistance = _attackDistance * _attackDistance;
            
            if (vector.sqrMagnitude <= sqrAttackDistance)
            {
                attackComponent.Attack(enemy);
                return BehaviourResult.Running;
            }
            
            Vector3 direction = vector.normalized;
            moveComponent.MoveStep(direction, deltaTime);
            return BehaviourResult.Running;
        }
        
        private GameObject FindNearestEnemyInRadius(Vector3 center, TeamType selfTeam)
        {
            Collider[] colliders = Physics.OverlapSphere(center, _detectRadius);
            GameObject nearestEnemy = null;
            float minSqrDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out TeamComponent team) && team.Team != selfTeam)
                {
                    float sqrDist = (collider.transform.position - center).sqrMagnitude;
                    if (sqrDist < minSqrDistance)
                    {
                        minSqrDistance = sqrDist;
                        nearestEnemy = collider.gameObject;
                    }
                }
            }

            return nearestEnemy;
        }
        
        private void ResetCommand() 
            => _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
    }
}