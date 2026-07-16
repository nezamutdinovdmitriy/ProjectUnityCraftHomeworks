using UnityEngine;
using Zenject;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class MoveComponentView : MonoBehaviour
    {
        private readonly int AnimatorKeyHash = Animator.StringToHash("IsMoving");
        
        private Animator _animator;
        
        private MoveRequestComponent _moveRequestComponent;

        [Inject]
        public void Construct(MoveRequestComponent moveRequestComponent) 
            => _moveRequestComponent = moveRequestComponent;

        private void Awake() => _animator = GetComponent<Animator>();

        private void OnEnable() => _moveRequestComponent.Moved += OnMoved;
        private void OnDisable() => _moveRequestComponent.Moved -= OnMoved;

        private void OnMoved(Vector2 direction)
        {
            bool isMoving = direction != Vector2.zero;
            _animator.SetBool(AnimatorKeyHash, isMoving);
        }
    }
}