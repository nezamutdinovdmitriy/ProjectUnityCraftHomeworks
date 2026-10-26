using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct UnitSpawnRandom : IComponentData
    {
        public Random Generator;
    }
}