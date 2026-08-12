using Atomic.Elements;
using Atomic.Entities;

namespace Game.GameEntities
{
    public class AttackTargetBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest _request;
        private IVariable<bool> _isReached;
        private IVariable<IGameEntity> _target;

        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.FireRequest);
            _isReached = entity.GetValue(GameEntityAPI.TargetIsReached);
            _target = entity.GetValue(GameEntityAPI.Target);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_target.Value == null)
                return;
            
            if (_target.Value.IsDead() == false
                && _isReached.Value)
                _request.Invoke();
        }
    }
}