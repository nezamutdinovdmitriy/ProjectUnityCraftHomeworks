using Atomic.Elements;
using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.GameEntity
{
    public class CharacterMovementController : IUIContextInit, IUIContextFixedTick
    {
        private Joystick _movementJoystick;

        private IRequest<Vector3> _movementRequest;
        private IVariable<Vector3> _movementDirection;
        
        public void Init(IUIContext context)
        {
            GameContext gameContext = GameContext.Instance;
            IGameEntity character = gameContext.GetValue(GameContextAPI.Character).Value;

            _movementRequest = character.GetValue(GameEntityAPI.MovementRequest);
            _movementDirection = character.GetValue(GameEntityAPI.MovementDirection);

            _movementJoystick = context.GetValue(UIContextAPI.MovementJoystick);
        }

        public void FixedTick(IUIContext entity, float deltaTime)
        {
            _movementDirection.Value =
                new Vector3(_movementJoystick.Direction.x, 0, _movementJoystick.Direction.y).normalized;
            
            if (_movementDirection.Value != Vector3.zero)
                _movementRequest.Invoke(_movementDirection.Value);
        }
    }
}