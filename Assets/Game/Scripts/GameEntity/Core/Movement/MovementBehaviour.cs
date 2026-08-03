using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public class MovementBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest<Vector3> _request;
        private ICommand<MovementArgs> _command;
        
        private IReactiveVariable<bool> _isMoving;
        private IValue<float> _speed;
        
        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.MovementRequest);
            _command = entity.GetValue(GameEntityAPI.MovementCommand);
            
            _isMoving = entity.GetValue(GameEntityAPI.IsMoving);
            _speed = entity.GetValue(GameEntityAPI.MovementSpeed);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_request.Consume(out Vector3 direction) == false)
            {
                _isMoving.Value = false;
                return;
            }
            
            MovementArgs args = new MovementArgs(direction, _speed.Value, deltaTime);

            _isMoving.Value = _command.TryInvoke(args);
        }
    }
}