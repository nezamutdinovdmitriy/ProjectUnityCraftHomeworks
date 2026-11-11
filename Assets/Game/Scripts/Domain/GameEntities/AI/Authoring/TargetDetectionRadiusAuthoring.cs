using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class TargetDetectionRadiusAuthoring : MonoBehaviour
    {
        [SerializeField] private float _detectRadius;
        
        private sealed class Baker : Baker<TargetDetectionRadiusAuthoring>
        {
            public override void Bake(TargetDetectionRadiusAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new TargetDetectionRadius {Value = authoring._detectRadius});
            }
        }
    }
}