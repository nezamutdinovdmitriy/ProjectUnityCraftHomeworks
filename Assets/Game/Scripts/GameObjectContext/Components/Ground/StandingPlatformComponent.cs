using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    public sealed class StandingPlatformComponent : IFixedTickable
    {
        private readonly GroundedComponent _groundedComponent;
        private readonly Transform _transform;
        
        private Transform _currentGround;

        public StandingPlatformComponent(GroundedComponent groundedComponent, Transform transform)
        {
            _groundedComponent = groundedComponent;
            _transform = transform;
        }
        
        public void FixedTick()
        {
            bool standing = _currentGround != null;
            bool hasPlatform = this.IsStanding(out Transform platform);

            if (!standing && hasPlatform)
            {
                _transform.parent = platform;
                _currentGround = platform;
            }

            if (standing && !hasPlatform)
            {
                _transform.parent = null;
                _currentGround = null;
            }
        }

        private bool IsStanding(out Transform platform)
        {
            platform = _groundedComponent.Ground;
            return platform && platform.CompareTag(GameObjectTags.Platform);
        }
    }
}