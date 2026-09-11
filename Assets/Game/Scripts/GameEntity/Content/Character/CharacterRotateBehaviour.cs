using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class CharacterRotateBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _aimDirection;

        private IRequest<Vector3> _rotateRequest;

        public void Init(IGameEntity entity)
        {
            _movementDirection = entity.GetValue(GameEntityAPI.MovementDirection);
            _aimDirection = entity.GetValue(GameEntityAPI.AimDirection);

            _rotateRequest = entity.GetValue(GameEntityAPI.RotateRequest);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            Vector3 direction = _aimDirection.Value != Vector3.zero
                ? _aimDirection.Value
                : _movementDirection.Value;

            if (direction != Vector3.zero)
                _rotateRequest.Invoke(direction);
        }
    }
}