using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class CharacterRotateBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private readonly GameContext _gameContext;
        
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _aimDirection;

        private IRequest<Vector3> _rotateRequest;

        public CharacterRotateBehaviour(GameContext gameContext) 
            => _gameContext = gameContext;

        public void Init(IGameEntity entity)
        {
            IGameEntity character = _gameContext.GetValue(GameContextAPI.Character).Value;

            _movementDirection = character.GetValue(GameEntityAPI.MovementDirection);
            _aimDirection = character.GetValue(GameEntityAPI.AimDirection);

            _rotateRequest = character.GetValue(GameEntityAPI.RotateRequest);
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