using System;
using UnityEngine;

namespace Game
{
    public sealed class MoveTransformComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Speed { get; private set; } = 4.5f;
        }

        private readonly Settings _settings;
        private readonly Transform _transform;

        public MoveTransformComponent(Settings settings, Transform transform)
        {
            _settings = settings;
            _transform = transform;
        }

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero) 
                _transform.Translate(
                    (Vector3) direction * _settings.Speed * Time.fixedDeltaTime,
                    Space.World);
        }
    }
}