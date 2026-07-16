using System;
using Game.Scripts.GameObjects;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Lava : IInitializable, IDisposable
    {
        private TriggerComponent _trigger;

        [Inject]
        private void Construct(TriggerComponent trigger) => _trigger = trigger;

        public void Initialize() => _trigger.OnEntered += this.OnTriggerEntered;

        public void Dispose() => _trigger.OnEntered -= this.OnTriggerEntered;
        
        private void OnTriggerEntered(Collider2D col)
        {
            Entity entity = col.GetComponentInParent<Entity>();
            
            if (entity != null 
                && entity.TryGet(out HealthComponent health))
                health.SetZero();
        }
    }
}