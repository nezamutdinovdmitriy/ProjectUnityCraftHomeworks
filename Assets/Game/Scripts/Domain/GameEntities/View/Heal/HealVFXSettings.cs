using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct HealVFXSettings : IComponentData
    {
        public Entity Prefab;
        public float3 Offset;
    }
}