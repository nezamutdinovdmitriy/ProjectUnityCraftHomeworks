using UnityEngine;
using Zenject;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class JumpComponentView : MonoBehaviour
    {
        private readonly int JumpAnimatorKeyHash = Animator.StringToHash("Jump");
        
        private JumpRequestComponent _jumpRequestComponent;
        
        private Animator _animator;

        [Inject]
        public void Construct(JumpRequestComponent jumpRequestComponent) 
            => _jumpRequestComponent = jumpRequestComponent;
        
        private void Awake() => _animator = GetComponent<Animator>();

        private void OnEnable() => _jumpRequestComponent.Jumped += OnJumped;
        private void OnDisable() => _jumpRequestComponent.Jumped -= OnJumped;
        
        private void OnJumped() => _animator.SetTrigger(JumpAnimatorKeyHash);
    }
}