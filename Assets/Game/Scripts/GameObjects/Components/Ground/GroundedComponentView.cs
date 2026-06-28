using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class GroundedComponentView : MonoBehaviour
    {
        private readonly int IsGroundedAnimatorKeyHash = Animator.StringToHash("IsGrounded");
        
        private GroundedComponent _groundedComponent;
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _groundedComponent = GetComponentInParent<GroundedComponent>();
        }

        private void FixedUpdate() 
            => _animator.SetBool(IsGroundedAnimatorKeyHash, _groundedComponent.IsGrounded);
    }
}