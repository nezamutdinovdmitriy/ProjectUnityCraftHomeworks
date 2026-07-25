using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.GameEntity.Content.Character
{
    public class CharacterInputController : IGameEntityInit, IGameEntityFixedTick
    {
        private Joystick _movementJoystick;
        private Joystick _aimJoystick;

        public void Init(IGameEntity entity)
        {
            UIContext uiContext = UIContext.Instance;

            _movementJoystick = uiContext.GetValue(UIContextAPI.MovementJoystick);
            _aimJoystick = uiContext.GetValue(UIContextAPI.AimJoystick);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            Vector3 movementDirection =
                new Vector3(_movementJoystick.Direction.x, 0, _movementJoystick.Direction.y).normalized;

            Vector3 aimDirection =
                new Vector3(_aimJoystick.Direction.x, 0, _aimJoystick.Direction.y).normalized;

            if (movementDirection != Vector3.zero)
                entity.GetValue(GameEntityAPI.MovementRequest).Invoke(movementDirection);

            if (aimDirection != Vector3.zero)
            {
                entity.GetValue(GameEntityAPI.RotateRequest).Invoke(aimDirection);

                if (entity.GetValue(GameEntityAPI.AimCooldown).IsCompleted())
                    entity.GetValue(GameEntityAPI.FireRequest).Invoke();
            }
        }
    }
}