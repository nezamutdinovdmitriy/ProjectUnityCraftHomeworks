using System;
using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    public sealed class ExtraGravityComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Gravity { get; private set; } = -7f;
        }

        private readonly Settings _settings;
        private readonly GroundedComponent _groundedComponent;
        private readonly Rigidbody2D _rigidbody;

        public ExtraGravityComponent(
            Settings settings, 
            GroundedComponent groundedComponent, 
            Rigidbody2D rigidbody)
        {
            _settings = settings;
            _groundedComponent = groundedComponent;
            _rigidbody = rigidbody;
        }


        public void FixedTick()
        {
            if (!_groundedComponent.IsGrounded)
                _rigidbody.linearVelocity += new Vector2(0, _settings.Gravity * Time.fixedDeltaTime);
        }
    }
}