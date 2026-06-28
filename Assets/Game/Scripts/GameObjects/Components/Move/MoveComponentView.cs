using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class MoveComponentView : MonoBehaviour
    {
        private readonly int AnimatorKeyHash = Animator.StringToHash("IsMoving");
        
        private MoveRequestComponent _moveComponent;
        private Animator _animator;
        
        private void Awake()
        {
            _moveComponent = GetComponentInParent<MoveRequestComponent>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable() => _moveComponent.Moved += OnMoved;
        private void OnDisable() => _moveComponent.Moved -= OnMoved;

        private void OnMoved(Vector2 direction)
        {
            bool isMoving = direction != Vector2.zero;
            _animator.SetBool(AnimatorKeyHash, isMoving);
        }
    }
}