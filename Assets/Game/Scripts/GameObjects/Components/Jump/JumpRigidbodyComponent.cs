using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpRigidbodyComponent : MonoBehaviour
    {
        [SerializeField]
        private float _jumpForce = 12f;

        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

        public void Jump() => _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}