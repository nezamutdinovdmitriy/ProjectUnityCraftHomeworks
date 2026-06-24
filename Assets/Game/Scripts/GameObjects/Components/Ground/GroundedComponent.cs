using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public sealed class GroundedComponent : MonoBehaviour
    {
        public event Action<bool> OnGrounded;
        
        [SerializeField]
        private Transform _feet;

        [SerializeField]
        private LayerMask _layerMask;
        
        [SerializeField]
        private float _groundDistance = 0.15f;
        
        [ShowInInspector, ReadOnly]
        private Transform _ground;
        
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private bool _isGrounded;

        public Transform Ground => _ground;
        
        public bool IsGrounded => _isGrounded;
        
        private void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(_feet.position, Vector2.down, _groundDistance, _layerMask);

            bool grounded = hit;
            if (grounded != _isGrounded)
            {
                _isGrounded = grounded;
                _ground = _isGrounded ? hit.transform : null;
                this.OnGrounded?.Invoke(_isGrounded);
            }
            else
            {
                _ground = _isGrounded ? hit.transform : null;
            }
        }

        private void OnDrawGizmos()
        {
            if (_feet != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_feet.position, _feet.position + Vector3.down * _groundDistance);
            }
        }
    }
}