using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class PatrolCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _detectRadius;

        [SerializeField]
        private float _attackDistance;

        [SerializeField]
        private float _stoppingDistance;

        [BlackboardValueKey(typeof(int))]
        private string _indexKey;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData) == false
                || commandData is not PatrolCommandData patrolCommandData
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out MoveComponent moveComponent) == false
                || character.TryGetComponent(out AttackComponent attackComponent) == false
                || character.TryGetComponent(out TeamComponent teamComponent) == false
                || _blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int patrolIndex) == false)
                return BehaviourResult.Failure;

            Vector3 selfPosition = character.transform.position;
            
            if (TryExtractPosition(
                    patrolCommandData, 
                    ref patrolIndex, 
                    out Vector3 targetPatrolPosition) == false)
                return BehaviourResult.Failure;
            
            GameObject enemy = FindNearestEnemyInRadius(selfPosition, teamComponent.Team);

            Vector3 vector = enemy != null
                ? enemy.transform.position - selfPosition
                : targetPatrolPosition - selfPosition;
            vector.y = 0;

            Vector3 direction = vector.normalized;

            float sqrDistanceToTarget = vector.sqrMagnitude;
            float sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
            float sqrAttackDistance = _attackDistance * _attackDistance;
            
            bool targetIsReached = sqrDistanceToTarget <= sqrStoppingDistance;
            bool targetInAttackDistance = sqrDistanceToTarget <= sqrAttackDistance;

            if (targetInAttackDistance && enemy != null)
            {
                attackComponent.Attack(enemy);
                return BehaviourResult.Running;
            }
            
            if (targetIsReached && enemy == null)
            {
                int nextIndex = (patrolIndex + 1) % patrolCommandData.Points.Count;

                _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, nextIndex);

                return BehaviourResult.Running;
            }

            moveComponent.MoveStep(direction, deltaTime);
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

        private bool TryExtractPosition(
            in PatrolCommandData commandData, 
            ref int index, 
            out Vector3 position)
        {
            position = default;

            if (commandData.Points == null
                || commandData.Points.Count == 0)
                return false;

            if (index >= commandData.Points.Count)
            {
                index %= commandData.Points.Count;
                _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, index);
            }
            
            PatrolCommandData.Point point = commandData.Points[index];
            
            if (point.Position.HasValue)
            {
                position = point.Position.Value;
                return true;
            }
            
            if (point.Target != null)
            {
                position = point.Target.transform.position;
                return true;
            }
            
            commandData.Points.RemoveAt(index);
            
            if (commandData.Points.Count == 0)
                return false;
            
            index %= commandData.Points.Count;
            _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, index);
            
            return TryExtractPosition(commandData, ref index, out position);
        }
    }
}