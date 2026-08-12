using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    public class DeathBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private ICooldown _delay;
        private IAction _action;
        
        public void Init(IGameEntity entity)
        {
            _delay = entity.GetValue(GameEntityAPI.DeathDelay);
            _action = entity.GetValue(GameEntityAPI.DeathAction);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (entity.IsDead())
                _delay.Tick(deltaTime);
            
            if(_delay.IsCompleted())
                _action.Invoke();
        }
    }
}