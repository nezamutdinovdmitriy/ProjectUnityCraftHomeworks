using Game.Patrol;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Platform : 
        PatrolComponent.ICondition,
        MoveRequestComponent.IAction,
        IInitializable
    {
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly MoveTransformComponent _moveTransformComponent;
        private readonly PatrolComponent _patrolComponent;
        private readonly FollowTargetComponent _followTargetComponent;

        public Platform(
            MoveTransformComponent transform, 
            PatrolComponent patrolComponent,
            FollowTargetComponent followTarget, 
            MoveRequestComponent moveRequestComponent)
        {
            _moveTransformComponent = transform;
            _patrolComponent = patrolComponent;
            _followTargetComponent = followTarget;
            _moveRequestComponent = moveRequestComponent;
        }

        public void Initialize() => MovementBehaviourSetup();

        private void MovementBehaviourSetup()
        {
            _moveRequestComponent.SetAction(this);
            _patrolComponent.SetCondition(this);
        }

        bool PatrolComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();

        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _moveTransformComponent.Move(direction);
        }
    }
}