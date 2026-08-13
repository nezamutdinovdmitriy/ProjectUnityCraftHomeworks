using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    public class TakeDamageComponentView : MonoBehaviour
    {
        [SerializeField]
        private TakeDamageColorComponent _takeDamageColorComponent;
        
        private HealthComponent _healthComponent;

        [Inject]
        public void Construct(HealthComponent healthComponent) 
            => _healthComponent = healthComponent;

        private void OnEnable() => _healthComponent.OnHealthChanged += OnHealthChanged;
        private void OnDisable() => _healthComponent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(float value) => _takeDamageColorComponent.TakeDamage();
    }
}