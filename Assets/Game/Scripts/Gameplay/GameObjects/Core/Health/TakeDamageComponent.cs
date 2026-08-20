using System;
using UnityEngine;

namespace SampleGame
{
    [RequireComponent(typeof(HealthComponent))]
    public sealed class TakeDamageComponent : MonoBehaviour
    {
        public event Action<TakeDamageArgs> OnDamageTaken;
        
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = this.GetComponent<HealthComponent>();
        }

        public bool TakeDamage(TakeDamageArgs args)
        {
            if (!_healthComponent.Decrement(args.damage))
                return false;

            this.OnDamageTaken?.Invoke(args);
            
            if (!_healthComponent.IsDead)
                return true;
            
            this.OnDamageTaken?.Invoke(args);
            return true;
        }
    }
}