using UnityEngine;
using Zenject;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class GroundedComponentView : MonoBehaviour
    {
        private readonly int IsGroundedAnimatorKeyHash = Animator.StringToHash("IsGrounded");
        
        private Animator _animator;
        
        private GroundedComponent _groundedComponent;

        [Inject]
        public void Construct(GroundedComponent groundedComponent) 
            => _groundedComponent = groundedComponent;
        
        private void Awake() => _animator = GetComponent<Animator>();

        private void FixedUpdate() 
            => _animator.SetBool(IsGroundedAnimatorKeyHash, _groundedComponent.IsGrounded);
    }
}