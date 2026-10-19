using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct VFXFollowTarget : IComponentData
    {
        public Entity Target;
        public float3 Offset;
    }
}