using Atomic.Elements;
using Atomic.Entities;
using Game.Bullets;
using UnityEngine;

namespace Game.GameEntity
{
    public class CollisionDamageBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private readonly IGameContext _gameContext;
        
        private IValue<float> _damage;
        private TriggerEvents _triggerEvents;
        private IGameEntity _self;

        public CollisionDamageBehaviour(IGameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void Init(IGameEntity entity)
        {
            _self = entity;
            
            _damage = entity.GetValue(GameEntityAPI.Damage);
            _triggerEvents = entity.GetValue(GameEntityAPI.Trigger);

            _triggerEvents.OnEntered += OnTriggerEntered;
        }

        public void Dispose(IGameEntity entity) => _triggerEvents.OnEntered -= OnTriggerEntered;

        private void OnTriggerEntered(Collider obj)
        {
            if (obj.TryGetComponent(out IGameEntity targetEntity) == false)
                return;

            targetEntity.TryTakeDamage(_damage.Value);
            
            _gameContext.DestroyBullet((GameEntity)_self);
        }
    }
}