using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using UnityEngine;

namespace Game.UI
{
    public class CharacterRotateController : IUIContextFixedTick, IUIContextInit
    {
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _aimDirection;

        private IRequest<Vector3> _rotateRequest;

        public void Init(IUIContext context)
        {
            GameContext gameContext = GameContext.Instance;
            IGameEntity character = gameContext.GetValue(GameContextAPI.Character).Value;

            _movementDirection = character.GetValue(GameEntityAPI.MovementDirection);
            _aimDirection = character.GetValue(GameEntityAPI.AimDirection);

            _rotateRequest = character.GetValue(GameEntityAPI.RotateRequest);
        }

        public void FixedTick(IUIContext context, float deltaTime)
        {
            Vector3 direction = _aimDirection.Value != Vector3.zero
                ? _aimDirection.Value
                : _movementDirection.Value;

            if (direction != Vector3.zero)
                _rotateRequest.Invoke(direction);
        }
    }
}