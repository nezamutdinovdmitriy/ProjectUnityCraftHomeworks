using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class DetectionRadiusAuthoring : MonoBehaviour
    {
        [SerializeField] private float _detectRadius;
        
        private sealed class Baker : Baker<DetectionRadiusAuthoring>
        {
            public override void Bake(DetectionRadiusAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new TargetDetectionRadius {Value = authoring._detectRadius});
            }
        }
    }
}