using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class MoveCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _stoppingDistance;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData command) == false
                || command is not MoveCommandData moveCommandData)
                return BehaviourResult.Failure;

            if (TryExtractPosition(moveCommandData, out Vector3 targetPosition) == false)
            {
                _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
                return BehaviourResult.Failure;
            }

            if (_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out MoveComponent moveComponent) == false)
                return BehaviourResult.Failure;
            
            Vector3 selfPosition = character.transform.position;
            Vector3 vector = targetPosition - selfPosition;
            vector.y = 0f;
            
            float sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
            bool isReached = vector.sqrMagnitude <= sqrStoppingDistance;
            
            if (isReached)
            {
                _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
                return BehaviourResult.Success;
            }

            Vector3 direction = vector.normalized;
            moveComponent.MoveStep(direction, deltaTime);
            
            return BehaviourResult.Running;
        }

        private bool TryExtractPosition(in MoveCommandData commandData, out Vector3 position)
        {
            if (commandData.Point.HasValue)
            {
                position = commandData.Point.Value;
                return true;
            }
            
            if (commandData.Target != null) 
            {
                position = commandData.Target.transform.position;
                return true;
            }

            position = default;
            return false;
        }
    }
}