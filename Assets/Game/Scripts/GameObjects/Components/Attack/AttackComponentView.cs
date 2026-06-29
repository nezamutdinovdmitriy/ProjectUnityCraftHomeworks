using UnityEngine;
using Zenject;

namespace Game
{
    public class AttackComponentView : MonoBehaviour
    {
        private int AnimatorKeyHash;
        
        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private string _animatorKeyString;
        
        [SerializeField]
        private ParticleSystem _particleSystem;
        
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _audioClip;
        
        private AttackRequestComponent _attackRequest;

        [Inject]
        public void Construct(AttackRequestComponent attackRequestComponent) 
            => _attackRequest = attackRequestComponent;

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