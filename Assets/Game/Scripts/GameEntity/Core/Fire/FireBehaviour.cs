using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    public class FireBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private readonly Cooldown _takeDamageDelay;

        private IRequest _request;
        private ICommand _command;
        private IEvent _fireStartEvent;

        public FireBehaviour(Cooldown takeDamageDelay) 
            => _takeDamageDelay = takeDamageDelay;

        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.FireRequest);
            _command = entity.GetValue(GameEntityAPI.FireCommand);
            _fireStartEvent = entity.GetValue(GameEntityAPI.FireStartEvent);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_takeDamageDelay.Duration <= 0 && _request.Consume())
            {
                _command.Invoke();
                return;
            }
            
            ProcessRequest(entity);
            ProcessDelay(deltaTime);
        }

        private void ProcessDelay(float deltaTime)
        {
            if (_takeDamageDelay.IsPlaying())
            {
                _takeDamageDelay.Tick(deltaTime);
                
                if(_takeDamageDelay.IsCompleted())
                    _command.Invoke();
            }
        }

        private void ProcessRequest(IGameEntity entity)
        {
            if (_request.Consume())
            {
                if (_takeDamageDelay.IsCompleted()
                    && _command.CanInvoke()
                    && entity.CanFireWeapon())
                {
                    _takeDamageDelay.ResetTime();
                    _fireStartEvent?.Invoke();
                }
            }
        }
    }
}