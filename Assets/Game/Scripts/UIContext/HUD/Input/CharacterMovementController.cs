using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using UnityEngine;

namespace Game.UI
{
    public class CharacterMovementController : IUIContextInit, IUIContextFixedTick
    {
        private readonly GameContext _gameContext;
        
        private Joystick _movementJoystick;

        private IRequest<Vector3> _movementRequest;
        private IVariable<Vector3> _movementDirection;

        public CharacterMovementController(GameContext gameContext) 
            => _gameContext = gameContext;

        public void Init(IUIContext context)
        {
            IGameEntity character = _gameContext.GetValue(GameContextAPI.Character).Value;

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