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
            if (_request.Consume(out var direction) == false)
            {
                _isMoving.Value = false;
                return;
            }
            
            MovementArgs args = new MovementArgs(direction, _speed.Value, deltaTime);

            if (_command.CanInvoke(args) == false)
            {
                _isMoving.Value = false;
                return;
            }

            _command.Invoke(args);
            _isMoving.Value = true;
        }
    }
}