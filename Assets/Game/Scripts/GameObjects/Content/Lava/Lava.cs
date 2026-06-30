using Game.Scripts.GameObjects;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Lava : MonoBehaviour
    {
        private TriggerComponent _trigger;

        [Inject]
        private void Construct(TriggerComponent trigger) => _trigger = trigger;

        private void OnEnable() => _trigger.OnEntered += this.OnTriggerEntered;

        private void OnDisable() => _trigger.OnEntered -= this.OnTriggerEntered;

        private void OnTriggerEntered(Collider2D col)
        {
            Entity entity = col.GetComponentInParent<Entity>();
            
            if (entity != null 
                && entity.TryGet(out HealthComponent health))
                health.SetZero();
        }
    }
}