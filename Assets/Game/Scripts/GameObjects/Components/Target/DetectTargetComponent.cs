using System;
using Game.Scripts.GameObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Target
{
    public class DetectTargetComponent : IInitializable, IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float DetectRadius { get; private set; }

            [field: SerializeField]
            public LayerMask TargetMask { get; private set; }
            
            [field: SerializeField]
            public int ColliderBufferCount { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TransformComponent _transform;
        
        [ShowInInspector, ReadOnly]
        private GameObject _currentTarget;

        private Collider2D[] _colliderBuffer;
        private ContactFilter2D _filter;

        public DetectTargetComponent(
            Settings settings, 
            TransformComponent transform)
        {
            _settings = settings;
            _transform = transform;
        }

        public void Initialize()
        {
            _colliderBuffer = new Collider2D[_settings.ColliderBufferCount];
            
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(_settings.TargetMask);
        }
        
        public void FixedTick() => DetectTarget();

        public bool TryGetTarget(out GameObject target)
        {
            if (_currentTarget == null)
            {
                target = null;
                return false;
            }

            target = _currentTarget;
            return true;
        }
        
        private void DetectTarget()
        {
            _currentTarget = null;
            
            int collidersCount = Physics2D.OverlapCircle(
                _transform.Position, 
                _settings.DetectRadius,
                _filter,
                _colliderBuffer);
            
            for (int i = 0; i < collidersCount; i++)
            {
                if (_colliderBuffer[i].TryGetComponent(out IEntity entity))
                {
                    _currentTarget = entity.Get<GameObject>();
                    break;
                }
            }
        }
        
        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.color = Color.blue;
        //
        //     Gizmos.DrawWireSphere(transform.position, _detectRadius);
        // }
    }
}