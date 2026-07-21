using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext
{
    public class MovementBehaviour : IEntityContextInit, IEntityContextFixedTick
    {
        private IRequest<Vector3> _request;
        private ICommand<Vector3, float> _command;
        
        public void Init(IEntityContext entity)
        {
            _request = entity.GetValue(EntityContextAPI.MovementRequest);
            _command = entity.GetValue(EntityContextAPI.MovementCommand);
        }

        public void FixedTick(IEntityContext entity, float deltaTime)
        {
            if(_request.Consume(out Vector3 direction))
                _command.Invoke(direction, deltaTime);
        }
    }
}