using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities.Weapon
{
    public class HandAttackBehaviour : IWeaponEntityInit, IWeaponEntityFixedTick
    {
        private readonly Cooldown _takeDamageDelay;
        
        private IRequest _request;
        private ICommand _command;
        private IEvent _attackStartEvent;

        private bool _inDelayProcess;

        public HandAttackBehaviour(Cooldown takeDamageDelay) 
            => _takeDamageDelay = takeDamageDelay;

        public void Init(IWeaponEntity weapon)
        {
            _request = weapon.GetValue(WeaponEntityAPI.FireRequest);
            _command = weapon.GetValue(WeaponEntityAPI.FireCommand);
            _attackStartEvent = weapon.GetValue(WeaponEntityAPI.FireStartEvent);
        }

        public void FixedTick(IWeaponEntity weapon, float deltaTime)
        {
            if (_inDelayProcess == false && _request.Required && _command.CanInvoke())
            {
                _takeDamageDelay.ResetTime();
                _attackStartEvent.Invoke();
                _inDelayProcess = true;
            }

            if (_inDelayProcess)
                _takeDamageDelay.Tick(deltaTime);
                
            if (_takeDamageDelay.IsCompleted())
            {
                _inDelayProcess = false;

                if (_request.Consume() && _command.CanInvoke())
                    weapon.GetValue(WeaponEntityAPI.FireCommand).Invoke();
            }
        }
    }
}