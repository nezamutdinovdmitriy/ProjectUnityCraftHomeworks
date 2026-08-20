using UnityEngine;

namespace SampleGame
{
    public sealed class TakeDamageAnimComponent : MonoBehaviour
    {
        private static readonly int TakeDamage = Animator.StringToHash(nameof(TakeDamage));

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private TakeDamageComponent _takeDamageComponent;

        private void OnEnable()
        {
            _takeDamageComponent.OnDamageTaken += this.OnDamageTaken;
        }

        private void OnDisable()
        {
            _takeDamageComponent.OnDamageTaken -= this.OnDamageTaken;
        }

        private void OnDamageTaken(TakeDamageArgs obj)
        {
            _animator.SetTrigger(TakeDamage);
        }
    }
}