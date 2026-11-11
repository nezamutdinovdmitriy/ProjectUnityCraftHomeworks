using Unity.Entities;

namespace Game.Components
{
    public struct UnitSpawnRequest : IBufferElementData
    {
        public Entity Prefab;
    }
}