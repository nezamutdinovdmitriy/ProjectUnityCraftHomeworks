using UnityEngine;

namespace SampleGame
{
    public sealed class DeathAnimComponent : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash(nameof(Death));

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private HealthComponent _healthComponent;

        private void OnEnable()
        {
            _healthComponent.OnDeath += this.OnDeath;
        }

        private void OnDisable()
        {
            _healthComponent.OnDeath -= this.OnDeath;
        }

        private void OnDeath()
        {
            _animator.SetTrigger(Death);
        }
    }
}