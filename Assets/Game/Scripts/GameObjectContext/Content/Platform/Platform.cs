using Game.Patrol;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Platform : 
        PatrolComponent.ICondition,
        IFixedTickable,
        IInitializable
    {
        private readonly MoveTransformComponent _transform;
        private readonly PatrolComponent _pointProvider;
        private readonly FollowTargetComponent _followTargetComponent;

        public Platform(
            MoveTransformComponent transform, 
            PatrolComponent pointProvider,
            FollowTargetComponent followTarget)
        {
            _transform = transform;
            _pointProvider = pointProvider;
            _followTargetComponent = followTarget;
        }

        public void Initialize() 
            => MovementBehaviourSetup();

        public void FixedTick()
        {
            if(_followTargetComponent.TryGetFollowDirection(out Vector2 direction))
                _transform.Move(direction);
        }

        private void MovementBehaviourSetup() 
            => _pointProvider.SetCondition(this);

        bool PatrolComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();
    }
}