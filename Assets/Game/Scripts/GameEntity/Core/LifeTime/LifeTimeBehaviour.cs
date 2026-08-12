using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntity.Core.LifeTime
{
    public class LifeTimeBehaviour : IGameEntityFixedTick, IGameEntityInit
    {
        private ICooldown _cooldown;
        
        public void Init(IGameEntity entity) 
            => _cooldown = entity.GetValue(GameEntityAPI.Lifetime);

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            _cooldown.Tick(deltaTime);
            
            if (_cooldown.IsCompleted())
                entity.ExpireLifetime();
        }
    }
}