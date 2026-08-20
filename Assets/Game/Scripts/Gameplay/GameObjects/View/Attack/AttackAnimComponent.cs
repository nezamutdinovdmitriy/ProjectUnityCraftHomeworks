using UnityEngine;

namespace SampleGame
{
    public sealed class AttackAnimComponent : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash(nameof(Fire));

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private AttackComponent _attackComponent;

        private void OnEnable()
        {
            _attackComponent.OnFire += this.OnFire;
        }

        private void OnDisable()
        {
            _attackComponent.OnFire -= this.OnFire;
        }

        private void OnFire()
        {
            _animator.SetTrigger(Fire);
        }
    }
}