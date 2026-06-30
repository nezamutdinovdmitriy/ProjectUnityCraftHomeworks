using System;
using Game.Scripts.GameObjects;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Trampoline : IInitializable, IDisposable
    {
        private readonly Vector2 _force;
        private readonly TriggerComponent _triggerComponent;

        public Trampoline(Vector2 force, TriggerComponent triggerComponent)
        {
            _force = force;
            _triggerComponent = triggerComponent;
        }
        
        public void Initialize() => _triggerComponent.OnEntered += this.OnEntered;

        public void Dispose() => _triggerComponent.OnEntered -= this.OnEntered;

        private void OnEntered(Collider2D other)
        {
            if (other.TryGetComponent(out Entity entity)
                && entity.TryGet(out Rigidbody2D rigidbody))
            {
                rigidbody.linearVelocityY = 0;
                rigidbody.AddForce(_force, ForceMode2D.Impulse);
            }
        }
    }
}