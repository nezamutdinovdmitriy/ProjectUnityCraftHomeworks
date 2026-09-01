using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    public class FollowTargetBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private readonly float _stoppingDistance;
        
        private IVariable<IGameEntity> _target;

        public FollowTargetBehaviour(float stoppingDistance) 
            => _stoppingDistance = stoppingDistance;

        public void Init(IGameEntity entity) 
            => _target = entity.GetValue(GameEntityAPI.Target);

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if ( _target.Value != null && _target.Value.IsDead() == false)
                entity.FollowToTarget(_target.Value, _stoppingDistance);
        }
    }
}