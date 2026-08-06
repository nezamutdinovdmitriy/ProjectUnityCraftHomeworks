using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntity.Core.Death
{
    public class DeathBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private ICooldown _delay;
        private ICommand _command;
        public void Init(IGameEntity entity)
        {
            _delay = entity.GetValue(GameEntityAPI.DeathDelay);
            _command = entity.GetValue(GameEntityAPI.DeathCommand);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (entity.IsDead())
                _delay.Tick(deltaTime);
            
            if(_delay.IsCompleted())
                _command.Invoke();
        }
    }
}