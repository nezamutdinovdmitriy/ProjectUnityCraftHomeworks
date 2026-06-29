using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class GroundedComponentView : MonoBehaviour
    {
        private readonly int IsGroundedAnimatorKeyHash = Animator.StringToHash("IsGrounded");
        
        [SerializeField]
        private GroundedComponent _groundedComponent;
        
        private Animator _animator;
        
        private void Awake() => _animator = GetComponent<Animator>();

        private void FixedUpdate() 
            => _animator.SetBool(IsGroundedAnimatorKeyHash, _groundedComponent.IsGrounded);
    }
}