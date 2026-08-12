using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities.Weapon
{
    public class PistolFireBehaviour : IWeaponEntityInit, IWeaponEntityFixedTick
    {
        private IRequest _request;
        private ICommand _command;
        
        public void Init(IWeaponEntity weapon)
        {
            _request = weapon.GetValue(WeaponEntityAPI.FireRequest);
            _command = weapon.GetValue(WeaponEntityAPI.FireCommand);
        }

        public void FixedTick(IWeaponEntity weapon, float deltaTime)
        {
            if(_request.Consume())
                _command.Invoke();
        }
    }
}