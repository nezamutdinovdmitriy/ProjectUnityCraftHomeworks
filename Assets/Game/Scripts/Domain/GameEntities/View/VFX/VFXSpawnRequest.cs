using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct VFXSpawnRequest : IBufferElementData
    {
        public Entity Prefab;
        public Entity Target;
        public float3 Offset;
        public bool FollowTarget;
    }
}