using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class TakeDamageComponentSFX : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _audioClip;
        
        private HealthComponent _healthComponent;

        [Inject]
        public void Construct(HealthComponent healthComponent) 
            => _healthComponent = healthComponent;

        private void OnEnable() => _healthComponent.OnHealthChanged += OnHealthChanged;
        private void OnDisable() => _healthComponent.OnHealthChanged += OnHealthChanged;

        private void OnHealthChanged(float obj)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}