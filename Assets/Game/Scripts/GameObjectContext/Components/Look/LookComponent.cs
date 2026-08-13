using UnityEngine;

namespace GameObjects.Components
{
    public sealed class LookComponent
    {
        private readonly Transform _transform;

        public LookComponent(Transform transform) => _transform = transform;

        public void Look(Transform target)
        {
            Vector2 direction = target.position - _transform.position;
            Look(direction.x);
        }
        
        public void Look(float direction)
        {
            float angle = direction > 0 ? 0 : 180;
            _transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}