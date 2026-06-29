using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class JumpComponentSFX : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _audioClip;

        private JumpRequestComponent _jumpRequestComponent;
        
        [Inject]
        public void Construct(JumpRequestComponent jumpRequestComponent) 
            => _jumpRequestComponent = jumpRequestComponent;

        private void Awake() => _jumpRequestComponent = GetComponentInParent<JumpRequestComponent>();

        private void OnEnable() => _jumpRequestComponent.Jumped += OnJumped;

        private void OnDisable() => _jumpRequestComponent.Jumped -= OnJumped;

        private void OnJumped()
        {
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}