using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public class CollisionDamageBehaviour : IGameEntityInit, IGameEntityDispose
    {
        private IValue<float> _damage;
        private TriggerEvents _triggerEvents;

        public void Init(IGameEntity entity)
        {
            _damage = entity.GetValue(GameEntityAPI.Damage);
            _triggerEvents = entity.GetValue(GameEntityAPI.Trigger);

            _triggerEvents.OnEntered += OnTriggerEntered;
        }

        public void Dispose(IGameEntity entity) => _triggerEvents.OnEntered -= OnTriggerEntered;

        private void OnTriggerEntered(Collider obj)
        {
            if (obj.TryGetComponent(out IGameEntity targetEntity) == false)
                return;

            targetEntity.TakeDamage(_damage.Value);
        }
    }
}