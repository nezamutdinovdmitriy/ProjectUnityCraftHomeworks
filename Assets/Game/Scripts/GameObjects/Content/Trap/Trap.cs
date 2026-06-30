using System;
using Game.Scripts.GameObjects;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Trap : IInitializable, IDisposable
    {
        private readonly int _damage;
        private readonly CollisionComponent _collisionComponent;

        public Trap(int damage, CollisionComponent collisionComponent)
        {
            _damage = damage;
            _collisionComponent = collisionComponent;
        }

        public void Initialize() => _collisionComponent.OnEntered += OnCollisionEntered;

        public void Dispose() => _collisionComponent.OnEntered -= OnCollisionEntered;

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Entity entity)
                && entity.TryGet(out HealthComponent health))
            {
                health.TakeDamage(_damage);
                GameObject.Destroy(_collisionComponent.gameObject);
            }
        }
    }
}