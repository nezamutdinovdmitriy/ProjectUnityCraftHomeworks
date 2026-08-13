using UnityEngine;

namespace GameObjects.Components
{
    public class JumpRigidbodyComponent
    {
        private readonly float _jumpForce;
        private readonly Rigidbody2D _rigidbody;

        public JumpRigidbodyComponent(float jumpForce, Rigidbody2D rigidbody)
        {
            _jumpForce = jumpForce;
            _rigidbody = rigidbody;
        }

        public void Jump() => _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}