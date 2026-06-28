using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class JumpComponentAnimationView : MonoBehaviour
    {
        private readonly int JumpAnimatorKeyHash = Animator.StringToHash("Jump");
        
        private JumpRequestComponent _jumpComponent;
        
        private Animator _animator;

        private void Awake()
        {
            _jumpComponent = GetComponentInParent<JumpRequestComponent>();
            
            _animator = GetComponent<Animator>();
        }

        private void OnEnable() => _jumpComponent.Jumped += OnJumped;
        private void OnDisable() => _jumpComponent.Jumped += OnJumped;
        
        private void OnJumped()
        {
            _animator.SetTrigger(JumpAnimatorKeyHash);

        }
    }
}