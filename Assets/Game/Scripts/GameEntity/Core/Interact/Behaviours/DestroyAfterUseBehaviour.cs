using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    public class DestroyAfterUseBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private readonly ICooldown _destroyDelay;
        
        private IVariable<bool> _wasUsed;
        private IGameEntity _self;

        public DestroyAfterUseBehaviour(ICooldown destroyDelay) 
            => _destroyDelay = destroyDelay;

        public void Init(IGameEntity entity)
        {
            _self = entity;
            _wasUsed = entity.GetValue(GameEntityAPI.WasUsed);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if(_wasUsed.Value)
                _destroyDelay.Tick(deltaTime);
                
            if(_destroyDelay.IsCompleted())
                GameEntity.Destroy(_self);
        }
    }
}