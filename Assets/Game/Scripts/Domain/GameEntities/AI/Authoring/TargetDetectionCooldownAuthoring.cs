using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class TargetDetectionCooldownAuthoring : MonoBehaviour
    {
        [SerializeField] private float _detectionCooldown;
        
        private sealed class Baker : Baker<TargetDetectionCooldownAuthoring>
        {
            public override void Bake(TargetDetectionCooldownAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new TargetDetectionCooldown
                {
                    Interval = authoring._detectionCooldown
                });
            }
        }
    }
}