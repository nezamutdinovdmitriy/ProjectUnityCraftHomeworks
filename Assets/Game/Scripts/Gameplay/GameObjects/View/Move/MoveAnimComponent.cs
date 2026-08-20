using UnityEngine;

namespace SampleGame
{
    public sealed class MoveAnimComponent : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash(nameof(IsMoving));

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private MoveComponent _moveComponent;

        private void Update()
        {
            _animator.SetBool(IsMoving, _moveComponent.IsMoving);
        }
    }
}