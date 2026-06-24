using UnityEngine;

namespace Game
{
    public sealed class Trampoline : MonoBehaviour
    {
        [SerializeField]
        private TriggerComponent _triggerComponent;

        [SerializeField]
        private Vector2 _force;
        
        private void OnEnable()
        {
            _triggerComponent.OnEntered += this.OnEntered;
        }

        private void OnDisable()
        {
            _triggerComponent.OnEntered -= this.OnEntered;
        }

        private void OnEntered(Collider2D other)
        {
            if (other.TryGetComponent(out Rigidbody2D rigidbody))
            {
                rigidbody.linearVelocityY = 0;
                rigidbody.AddForce(_force, ForceMode2D.Impulse);
            }
        }
    }
}