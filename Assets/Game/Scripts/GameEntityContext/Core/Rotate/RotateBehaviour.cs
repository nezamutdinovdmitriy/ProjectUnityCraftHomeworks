using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class RotateBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest<Vector3> _request;
        private ICommand<RotateArgs> _command;
        
        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.RotateRequest);
            _command = entity.GetValue(GameEntityAPI.RotateCommand);
        }
        
        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_request.Consume(out Vector3 direction))
            {
                _command.Invoke(new RotateArgs(
                    direction,
                    entity.GetValue(GameEntityAPI.RotateSpeed).Value,
                    deltaTime));
            }   
        }
    }
}