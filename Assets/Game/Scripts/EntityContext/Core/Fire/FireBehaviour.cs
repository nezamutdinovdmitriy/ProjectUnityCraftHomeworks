using Atomic.Elements;
using Atomic.Entities;

namespace Game.EntityContext.Core.Fire
{
    public class FireBehaviour : IEntityContextInit, IEntityContextTick
    {
        private IRequest _request;
        private ICommand _command;
        
        public void Init(IEntityContext entity)
        {
            _request = entity.GetValue(EntityContextAPI.FireRequest);
            _command = entity.GetValue(EntityContextAPI.FireCommand);
        }

        public void Tick(IEntityContext entity, float deltaTime)
        {
            if (_request.Consume())
                _command.Invoke();
        }
    }
}