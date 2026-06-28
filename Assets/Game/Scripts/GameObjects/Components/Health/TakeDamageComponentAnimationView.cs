using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class TakeDamageComponentAnimationView : MonoBehaviour
    {
        [SerializeField]
        private TakeDamageColorComponent _takeDamageColorComponent;
        
        private HealthComponent _healthComponent;

        private void Awake() => _healthComponent = GetComponentInParent<HealthComponent>();

        private void OnEnable() => _healthComponent.OnHealthChanged += OnHealthChanged;
        private void OnDisable() => _healthComponent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(float value) => _takeDamageColorComponent.TakeDamage();
    }
}