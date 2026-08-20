using UnityEngine;

namespace SampleGame
{
    public sealed class DeathSFXBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _deathClip;

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
            _audioSource.PlayOneShot(_deathClip);
        }
    }
}