using Game.Patrol;
using Game.Target;
using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour,
        PointProviderComponent.ICondition,
        PointProviderComponent.IAction
    {
        private MoveTransformComponent _moveTransformComponent;
        private PointProviderComponent _pointProviderComponent;
        private FollowTargetComponent _followTargetComponent;
        
        private void Awake() => MovementBehaviourSetup();

        private void Start() => _followTargetComponent.SetTargetPoint(_pointProviderComponent.GetPoint());

        private void FixedUpdate()
        {
            if (_followTargetComponent.IsDestinationReached())
                return;
            
            _moveTransformComponent.Move(_followTargetComponent.GetDirectionToTarget());
        }

        private void MovementBehaviourSetup()
        {
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            _followTargetComponent = GetComponent<FollowTargetComponent>();
            
            _pointProviderComponent = GetComponent<PointProviderComponent>();
            _pointProviderComponent.SetAction(this);
            _pointProviderComponent.SetCondition(this);
        }
        
        bool PointProviderComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();

        void PointProviderComponent.IAction.Invoke() 
            => _followTargetComponent.SetTargetPoint(_pointProviderComponent.GetPoint());
    }
}