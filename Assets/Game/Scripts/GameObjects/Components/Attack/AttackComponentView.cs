using UnityEngine;

namespace Game
{
    public class AttackComponentView : MonoBehaviour
    {
        private int AnimatorKeyHash;
        
        [SerializeField]
        private string _animatorKeyString;
        
        [SerializeField]
        private AttackRequestComponent _attackRequest;

        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _audioClip;

        private void Awake() => AnimatorKeyHash = Animator.StringToHash(_animatorKeyString);

        private void OnEnable() => _attackRequest.Attacked += OnAttacked;
        private void OnDisable() => _attackRequest.Attacked -= OnAttacked;

        private void OnAttacked()
        {
            _animator.SetTrigger(AnimatorKeyHash);
            _audioSource.PlayOneShot(_audioClip);
            _particleSystem.Play();
        }
    }
}