using Unity.Entities;

namespace Game.Components
{
    public struct UnitSpawnPoint : IBufferElementData
    {
        public Entity Value;
    }
}