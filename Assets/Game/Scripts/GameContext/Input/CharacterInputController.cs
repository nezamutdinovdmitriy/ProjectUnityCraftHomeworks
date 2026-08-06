using Atomic.Elements;
using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.GameEntity
{
    public class CharacterInputController : IGameContextInit, IGameContextFixedTick
    {
        private Joystick _movementJoystick;
        private Joystick _aimJoystick;

        private IVariable<IGameEntity> _character;

        public void Init(IGameContext entity)
        {
            UIContext uiContext = UIContext.Instance;

            _movementJoystick = uiContext.GetValue(UIContextAPI.MovementJoystick);
            _aimJoystick = uiContext.GetValue(UIContextAPI.AimJoystick);

            _character = entity.GetValue(GameContextAPI.Character);
        }

        public void FixedTick(IGameContext entity, float deltaTime)
        {
            Vector3 movementDirection =
                new Vector3(_movementJoystick.Direction.x, 0, _movementJoystick.Direction.y).normalized;
            
            Vector3 aimDirection =
                new Vector3(_aimJoystick.Direction.x, 0, _aimJoystick.Direction.y).normalized;

            if (movementDirection != Vector3.zero)
                _character.Value.GetValue(GameEntityAPI.MovementRequest).Invoke(movementDirection);

            if (aimDirection != Vector3.zero)
                _character.Value.GetValue(GameEntityAPI.RotateRequest).Invoke(aimDirection);
            else if (movementDirection != Vector3.zero)
                _character.Value.GetValue(GameEntityAPI.RotateRequest).Invoke(movementDirection);
        }
    }
}