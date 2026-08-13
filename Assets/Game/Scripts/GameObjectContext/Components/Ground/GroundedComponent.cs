using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;

namespace GameObjects.Components
{
    [Serializable]
    public sealed class GroundedComponent : IFixedTickable
    {
        [Serializable]
        public sealed class Settings
        {
            [field: SerializeField]
            public Collider2D Collider { get; private set; }

            [field: SerializeField]
            public LayerMask GroundMask { get; private set; }

            [field: SerializeField]
            [field: Range(0f, 1f)]
            public float GroundNormalThreshold { get; private set; } = 0.5f;
        }

        public event Action<bool> OnGrounded;

        public bool IsGrounded => _isGrounded;

        public Transform Ground => _ground;

        public Settings CurrentSettings => _settings;

        private readonly Settings _settings;
        private readonly ContactPoint2D[] _contacts = new ContactPoint2D[16];

        [ShowInInspector, ReadOnly]
        private Transform _ground;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private bool _isGrounded;

        public GroundedComponent(Settings settings)
        {
            _settings = settings;
        }

        public void FixedTick()
        {
            bool grounded = false;
            Transform ground = null;

            int contactCount = _settings.Collider.GetContacts(_contacts);

            for (int i = 0; i < contactCount; i++)
            {
                ContactPoint2D contact = _contacts[i];

                if (((1 << contact.collider.gameObject.layer) & _settings.GroundMask.value) == 0)
                    continue;

                if (contact.normal.y < _settings.GroundNormalThreshold)
                    continue;

                grounded = true;
                ground = contact.collider.transform;
                break;
            }

            if (grounded != _isGrounded)
            {
                _isGrounded = grounded;
                OnGrounded?.Invoke(_isGrounded);
            }

            _ground = ground;
        }
    }
}