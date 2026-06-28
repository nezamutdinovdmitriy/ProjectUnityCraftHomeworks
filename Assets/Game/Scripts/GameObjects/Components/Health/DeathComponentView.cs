using UnityEngine;

namespace Game
{
    public class DeathComponentView : MonoBehaviour
    {
        private readonly int DeathAnimatorKeyHash = Animator.StringToHash("Death");
        
        private Animator _animator;
        private HealthComponent _healthComponent;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _healthComponent = GetComponentInParent<HealthComponent>();
        }

        private void OnEnable() => _healthComponent.OnDied += OnDied;
        private void OnDisable() => _healthComponent.OnDied -= OnDied;

        private void OnDied() => _animator.SetTrigger(DeathAnimatorKeyHash);
    }
}