using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.EntityContext
{
    public class CharacterInputController : IEntityContextInit, IEntityContextFixedTick
    {
        private Joystick _movementJoystick;
        private Joystick _fireJoystick;
        
        public void Init(IEntityContext entity)
        {
            UIContext uiContext = UIContext.Instance;
            _movementJoystick = uiContext.GetValue(UIContextAPI.MovementJoystick);
            _fireJoystick = uiContext.GetValue(UIContextAPI.FireJoystick);
        }
        
        public void FixedTick(IEntityContext entity, float deltaTime)
        {
            Vector3 movementJoystickDirection =
                new Vector3(_movementJoystick.Direction.x, 0, _movementJoystick.Direction.y).normalized;
            
            Vector3 fireJoystickDirection =
                new Vector3(_fireJoystick.Direction.x, 0, _fireJoystick.Direction.y).normalized;
            
            entity.GetValue(EntityContextAPI.MovementRequest).Invoke(movementJoystickDirection);

            if (_fireJoystick.Direction == Vector2.zero)
                entity.GetValue(EntityContextAPI.RotationRequest).Invoke(movementJoystickDirection);

            if (_fireJoystick.Direction != Vector2.zero)
            {
                entity.GetValue(EntityContextAPI.RotationRequest).Invoke(fireJoystickDirection);
                entity.GetValue(EntityContextAPI.FireRequest).Invoke();
            }
        }
    }
}