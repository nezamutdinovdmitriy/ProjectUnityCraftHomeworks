using UnityEngine;

namespace Game
{
    public sealed class StandingPlatformComponent : MonoBehaviour
    {
        [SerializeField]
        private GroundedComponent _groundedComponent;

        private Transform _currentGround;
        
        private void FixedUpdate()
        {
            bool standing = _currentGround != null;
            bool hasPlatform = this.IsStanding(out Transform platform);

            if (!standing && hasPlatform)
            {
                this.transform.parent = platform;
                _currentGround = platform;
            }

            if (standing && !hasPlatform)
            {
                this.transform.parent = null;
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