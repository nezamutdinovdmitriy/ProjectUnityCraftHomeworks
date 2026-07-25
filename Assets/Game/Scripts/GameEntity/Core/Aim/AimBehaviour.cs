using Atomic.Elements;
using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.GameEntity.Core.Aim
{
    public class AimBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private Joystick _joystick;
        private ICooldown _cooldown;
        private IVariable<bool> _hasAimingLastFrame;
        
        public void Init(IGameEntity entity)
        {
            _cooldown = entity.GetValue(GameEntityAPI.AimCooldown);
            _hasAimingLastFrame = entity.GetValue(GameEntityAPI.HasAimingLastFrame);
        }
        
        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_joystick == null && TryGetJoystick() == false)
                return;
            
            bool isAiming = _joystick.Direction != Vector2.zero;

            _cooldown.Tick(deltaTime);

            if (isAiming && _hasAimingLastFrame.Value == false)
                _cooldown.ResetTime();

            _hasAimingLastFrame.Value = isAiming;
        }

        private bool TryGetJoystick()
        {
            if (UIContext.Instance == null)
                return false;
            
            _joystick = UIContext.Instance.GetValue(UIContextAPI.AimJoystick);
            return _joystick != null;
        }
    }
}