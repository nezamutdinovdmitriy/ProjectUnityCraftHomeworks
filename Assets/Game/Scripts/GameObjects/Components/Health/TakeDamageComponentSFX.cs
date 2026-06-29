using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class TakeDamageComponentSFX : MonoBehaviour
    {
        private readonly int DeathAnimatorKeyHash = Animator.StringToHash("Death");
        
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _audioClip;
        
        private HealthComponent _healthComponent;

        private void Awake() => _healthComponent = GetComponentInParent<HealthComponent>();

        private void OnEnable() => _healthComponent.OnHealthChanged += OnHealthChanged;
        private void OnDisable() => _healthComponent.OnHealthChanged += OnHealthChanged;

        private void OnHealthChanged(float obj)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}