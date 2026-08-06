using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntity
{
    public class AttackTargetBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest _request;
        private IVariable<bool> _isReached;

        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.FireRequest);
            _isReached = entity.GetValue(GameEntityAPI.TargetIsReached);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_isReached.Value)
                _request.Invoke();
        }
    }
}