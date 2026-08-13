using System;
using Game.Scripts.GameObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameObjects.Components
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
        private readonly Transform _transform;

        [ShowInInspector, ReadOnly]
        private readonly TargetComponent _targetComponent;

        private Collider2D[] _colliderBuffer;
        private ContactFilter2D _filter;

        public DetectTargetComponent(Settings settings, Transform transform, 
            TargetComponent targetComponent)
        {
            _settings = settings;
            _transform = transform;
            _targetComponent = targetComponent;
        }

        public void Initialize()
        {
            _colliderBuffer = new Collider2D[_settings.ColliderBufferCount];
            
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(_settings.TargetMask);
        }
        
        public void FixedTick() => DetectTarget();
        
        private void DetectTarget()
        {
            _targetComponent.Target = null;
            
            int collidersCount = Physics2D.OverlapCircle(
                _transform.position, 
                _settings.DetectRadius,
                _filter,
                _colliderBuffer);
            
            for (int i = 0; i < collidersCount; i++)
            {
                Collider2D collider = _colliderBuffer[i];

                if (collider.TryGetComponent(out Entity entity))
                {
                    _targetComponent.Target = entity.gameObject;
                    break;
                }
            }
        }
    }
}