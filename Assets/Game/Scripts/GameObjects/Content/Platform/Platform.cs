using Game.Patrol;
using Game.Target;
using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour,
        PatrolComponent.ICondition,
        PatrolComponent.IAction
    {
        private MoveTransformComponent _moveTransformComponent;
        private PatrolComponent _patrolComponent;
        private FollowTargetComponent _followTargetComponent;
        
        private void Awake()
        {
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            
            _patrolComponent = GetComponent<PatrolComponent>();
            _patrolComponent.SetAction(this);
            _patrolComponent.SetCondition(this);
            
            _followTargetComponent = GetComponent<FollowTargetComponent>();
        }

        private void Start() => _followTargetComponent.SetTargetPoint(_patrolComponent.GetPoint());

        private void FixedUpdate()
        {
            if (_followTargetComponent.IsDestinationReached())
                return;
            
            _moveTransformComponent.Move(_followTargetComponent.GetDirectionToTarget());
        }

        bool PatrolComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();

        void PatrolComponent.IAction.Invoke() 
            => _followTargetComponent.SetTargetPoint(_patrolComponent.GetPoint());
    }
}