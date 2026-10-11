using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public sealed class TargetHitPointAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform _hitPoint;
        
        private sealed class Baker : Baker<TargetHitPointAuthoring>
        {
            public override void Bake(TargetHitPointAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new TargetHitPoint
                {
                    Value = GetEntity(authoring._hitPoint, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}