using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    [RequireComponent(typeof(Animator))]
    public class DeathComponentView : MonoBehaviour
    {
        private readonly int DeathAnimatorKeyHash = Animator.StringToHash("Death");
        
        private Animator _animator;
        private HealthComponent _healthComponent;

        [Inject]
        public void Construct(HealthComponent healthComponent) 
            => _healthComponent = healthComponent;

        private void Awake() => _animator = GetComponent<Animator>();

        private void OnEnable() => _healthComponent.OnDied += OnDied;

        private void OnDisable() => _healthComponent.OnDied -= OnDied;

        private void OnDied() => _animator.SetTrigger(DeathAnimatorKeyHash);
    }
}