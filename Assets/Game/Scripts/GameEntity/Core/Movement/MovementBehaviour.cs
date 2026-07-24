using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public class MovementBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest<Vector3> _request;
        private ICommand<MovementArgs> _command;
        
        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.MovementRequest);
            _command = entity.GetValue(GameEntityAPI.MovementCommand);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if(_request.Consume(out Vector3 direction))
                _command.Invoke(new MovementArgs(
                    direction, 
                    entity.GetValue(GameEntityAPI.MovementSpeed).Value,
                    deltaTime));
        }
    }
}