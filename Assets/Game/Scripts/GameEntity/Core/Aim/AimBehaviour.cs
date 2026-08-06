using Atomic.Elements;
using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.GameEntity
{
    public class AimBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private Joystick _joystick;
        private ICooldown _cooldown;
        private IVariable<bool> _hasAimingLastFrame;
        private IRequest _fireRequest;
        
        public void Init(IGameEntity entity)
        {
            _cooldown = entity.GetValue(GameEntityAPI.AimCooldown);
            _hasAimingLastFrame = entity.GetValue(GameEntityAPI.HasAimingLastFrame);
            _fireRequest = entity.GetValue(GameEntityAPI.FireRequest);
        }
        
        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_joystick == null && TryGetJoystick() == false)
                return;
            
            bool isAiming = _joystick.Direction != Vector2.zero;

            if (isAiming)
            {
                if (!_hasAimingLastFrame.Value)
                    _cooldown.ResetTime();

                _cooldown.Tick(deltaTime);

                if (_cooldown.IsCompleted())
                    _fireRequest.Invoke();
            }
            
            _hasAimingLastFrame.Value = isAiming;
            
            // _cooldown.Tick(deltaTime);
            //
            // if (isAiming && _hasAimingLastFrame.Value == false)
            //     _cooldown.ResetTime();
            //
            // _hasAimingLastFrame.Value = isAiming;
            
            Debug.Log(isAiming);
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