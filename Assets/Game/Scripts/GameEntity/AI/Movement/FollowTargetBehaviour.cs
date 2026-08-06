using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public class FollowTargetBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private readonly float _stoppingDistance;

        private IVariable<bool> _isReached;
        private IVariable<IGameEntity> _target;

        public FollowTargetBehaviour(float stoppingDistance) 
            => _stoppingDistance = stoppingDistance;

        public void Init(IGameEntity entity)
        {
            _target = entity.GetValue(GameEntityAPI.Target);
            _isReached = entity.GetValue(GameEntityAPI.TargetIsReached);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if ( _target.Value != null && _target.Value.IsDead() == false)
            {
                Vector3 targetPosition = _target.Value.GetValue(GameEntityAPI.Position).Value;
                Vector3 selfPosition = entity.GetValue(GameEntityAPI.Position).Value;
                
                Vector3 moveDirection = (targetPosition - selfPosition).normalized;

                _isReached.Value = (targetPosition - selfPosition).magnitude <= _stoppingDistance;

                if (_isReached.Value == false)
                    entity.GetValue(GameEntityAPI.MovementRequest).Invoke(moveDirection);

                entity.GetValue(GameEntityAPI.RotateRequest).Invoke(moveDirection);
            }
        }
    }
}