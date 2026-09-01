using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using UnityEngine;

namespace Game.UI
{
    public class CharacterAimController : IUIContextInit, IUIContextFixedTick
    {
        private readonly GameContext _gameContext;
        
        private Joystick _aimJoystick;
        
        private IVariable<Vector3> _aimDirection;

        public CharacterAimController(GameContext gameContext)
            => _gameContext = gameContext;

        public void Init(IUIContext context)
        {
            IGameEntity character = _gameContext.GetValue(GameContextAPI.Character).Value;

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