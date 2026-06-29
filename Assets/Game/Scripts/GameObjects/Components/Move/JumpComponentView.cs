using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class JumpComponentView : MonoBehaviour
    {
        private readonly int JumpAnimatorKeyHash = Animator.StringToHash("Jump");
        
        private JumpRequestComponent _jumpRequestComponent;
        
        private Animator _animator;

        private void Awake()
        {
            _jumpRequestComponent = GetComponentInParent<JumpRequestComponent>();
            
            _animator = GetComponent<Animator>();
        }

        private void OnEnable() => _jumpRequestComponent.Jumped += OnJumped;
        private void OnDisable() => _jumpRequestComponent.Jumped += OnJumped;
        
        private void OnJumped() => _animator.SetTrigger(JumpAnimatorKeyHash);
    }
}