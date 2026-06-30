using Game.Patrol;
using Game.Target;
using Zenject;

namespace Game
{
    public class Platform : 
        PointProviderComponent.ICondition,
        PointProviderComponent.IAction, 
        IFixedTickable,
        IInitializable
    {
        private readonly MoveTransformComponent _transform;
        private readonly PointProviderComponent _pointProvider;
        private readonly FollowTargetComponent _followTarget;

        public Platform(
            MoveTransformComponent transform, 
            PointProviderComponent pointProvider,
            FollowTargetComponent followTarget)
        {
            _transform = transform;
            _pointProvider = pointProvider;
            _followTarget = followTarget;
        }

        public void Initialize()
        {
            MovementBehaviourSetup();
            _followTarget.SetTargetPoint(_pointProvider.GetPoint());
        }

        public void FixedTick()
        {
            if (_followTarget.IsDestinationReached())
                return;
            
            _transform.Move(_followTarget.GetDirectionToTarget());
        }

        private void MovementBehaviourSetup()
        {
            _pointProvider.SetAction(this);
            _pointProvider.SetCondition(this);
        }
        
        bool PointProviderComponent.ICondition.Evaluate() 
            => _followTarget.IsDestinationReached();

        void PointProviderComponent.IAction.Invoke() 
            => _followTarget.SetTargetPoint(_pointProvider.GetPoint());
    }
}