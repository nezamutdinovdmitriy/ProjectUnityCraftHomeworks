using System;
using Game.Scripts.GameObjects;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
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