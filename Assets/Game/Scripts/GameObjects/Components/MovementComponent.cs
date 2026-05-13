using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class MovementComponent
    {
        public event Action<Vector3> Moved;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private float _speed;

        public void SetSpeed(float speed) => _speed = speed;

        public void MoveStep(Vector2? direction, float deltaTime)
        {
            if (direction.HasValue == false)
                return;
            
            Vector2 newPosition = _rigidbody.position + direction.Value * (_speed * deltaTime);
            _rigidbody.MovePosition(newPosition);
            direction = null;

            Moved?.Invoke(direction.Value);
        }
    }
}