using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ForceAttackComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float ForceX { get; private set; } = 10f;

            [field: SerializeField]
            public float ForceY { get; private set; } = 2f;

            [field: SerializeField]
            public Vector2 BoxSize { get; private set; } = new(1.5f, 1f);

            [field: SerializeField]
            public Vector2 Offset { get; private set; } = new(1f, 0f);

            [field: SerializeField]
            public LayerMask TargetMask { get; private set; }
            
            [field: SerializeField]
            public int ColliderMaxCount { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TransformComponent _transform;
        private readonly Collider2D[] _hits;

        public ForceAttackComponent(Settings settings, TransformComponent transform)
        {
            _settings = settings;
            _transform = transform;
            
            _hits = new Collider2D[_settings.ColliderMaxCount];
        }
        
        [Obsolete]
        public void Attack()
        {
            Vector2 center =
                (Vector2) _transform.Position +
                (Vector2) _transform.Right * _settings.Offset.x +
                Vector2.up * _settings.Offset.y;
            
            int count = Physics2D.OverlapBoxNonAlloc(
                center,
                _settings.BoxSize,
                0f,
                _hits,
                _settings.TargetMask);

            for (int i = 0; i < count; i++)
            {
                Rigidbody2D rb = _hits[i].attachedRigidbody;

                if (rb == null)
                    continue;

                Vector2 dir = (rb.position - (Vector2) _transform.Position).normalized;
                Vector2 force = new Vector2(
                    dir.x * _settings.ForceX,
                    _settings.ForceY);
                
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        
        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.color = Color.red;
        //     
        //     Vector2 center =
        //         (Vector2)transform.position +
        //         (Vector2)transform.right * _offset.x +
        //         Vector2.up * _offset.y;
        //
        //     Gizmos.DrawWireCube(center, _boxSize);
        // }
    }
}