using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using UnityEngine;

namespace Game.UI
{
    public class CharacterAimController : IUIContextInit, IUIContextFixedTick
    {
        private Joystick _aimJoystick;
        
        private IVariable<Vector3> _aimDirection;
        
        public void Init(IUIContext context)
        {
            GameContext gameContext = GameContext.Instance;
            IGameEntity character = gameContext.GetValue(GameContextAPI.Character).Value;

            _aimDirection = character.GetValue(GameEntityAPI.AimDirection);
            
            _aimJoystick = context.GetValue(UIContextAPI.AimJoystick);
        }

        public void FixedTick(IUIContext entity, float deltaTime)
        {
            Vector3 aimDirection =
                new Vector3(_aimJoystick.Direction.x, 0, _aimJoystick.Direction.y).normalized;

            _aimDirection.Value = aimDirection;
        }
    }
}