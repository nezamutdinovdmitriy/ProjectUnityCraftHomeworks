using System;
using UnityEngine;

namespace Game
{
    public class ForceAttackComponent : MonoBehaviour
    {
        [SerializeField]
        private float _forceX = 10f;

        [SerializeField]
        private float _forceY = 2f;

        [SerializeField]
        private Vector2 _boxSize = new(1.5f, 1f);

        [SerializeField]
        private Vector2 _offset = new(1f, 0f);

        [SerializeField]
        private LayerMask _targetMask;
        
        private readonly Collider2D[] _hits = new Collider2D[6];

        [Obsolete]
        public void Attack()
        {
            Vector2 center =
                (Vector2)transform.position +
                (Vector2)transform.right * _offset.x +
                Vector2.up * _offset.y;
            
            int count = Physics2D.OverlapBoxNonAlloc(
                center,
                _boxSize,
                0f,
                _hits,
                _targetMask);

            for (int i = 0; i < count; i++)
            {
                Rigidbody2D rb = _hits[i].attachedRigidbody;

                if (rb == null)
                    continue;

                Vector2 dir = (rb.position - (Vector2)transform.position).normalized;
                Vector2 force = new Vector2(
                    dir.x * _forceX,
                    _forceY);
                
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            
            Vector2 center =
                (Vector2)transform.position +
                (Vector2)transform.right * _offset.x +
                Vector2.up * _offset.y;

            Gizmos.DrawWireCube(center, _boxSize);
        }
    }
}