using UnityEngine;

namespace Game
{
    public sealed class LookComponent
    {
        private readonly TransformComponent _transform;

        public LookComponent(TransformComponent transform) => _transform = transform;

        public void Look(Transform target)
        {
            Vector2 direction = target.position - _transform.Position;
            Look(direction.x);
        }
        
        public void Look(float direction)
        {
            float angle = direction > 0 ? 0 : 180;
            _transform.EulerAngles = new Vector3(0, angle, 0);
        }
    }
}