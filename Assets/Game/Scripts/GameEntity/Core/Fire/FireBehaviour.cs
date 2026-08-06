using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntity
{
    public class FireBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest _request;
        private ICommand _command;
        
        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.FireRequest);
            _command = entity.GetValue(GameEntityAPI.FireCommand);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_request.Consume())
                _command.Invoke();
        }
    }
}