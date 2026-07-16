using System;
using UnityEngine;

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
        private readonly Transform _transform;
        private readonly Collider2D[] _hits;
        private readonly ContactFilter2D _contactFilter;

        public ForceAttackComponent(Settings settings, Transform transform)
        {
            _settings = settings;
            _transform = transform;
            
            _hits = new Collider2D[_settings.ColliderMaxCount];
            
            _contactFilter = new ContactFilter2D();
            _contactFilter.SetLayerMask(_settings.TargetMask);
        }
        
        public void Attack()
        {
            Vector2 center =
                (Vector2) _transform.position +
                (Vector2) _transform.right * _settings.Offset.x +
                Vector2.up * _settings.Offset.y;

            int count = Physics2D.OverlapBox(
                center, 
                _settings.BoxSize, 
                0f, 
                _contactFilter, 
                _hits);

            for (int i = 0; i < count; i++)
            {
                Rigidbody2D rb = _hits[i].attachedRigidbody;

                if (rb == null)
                    continue;

                Vector2 dir = (rb.position - (Vector2) _transform.position).normalized;
                Vector2 force = new Vector2(
                    dir.x * _settings.ForceX,
                    _settings.ForceY);
                
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}