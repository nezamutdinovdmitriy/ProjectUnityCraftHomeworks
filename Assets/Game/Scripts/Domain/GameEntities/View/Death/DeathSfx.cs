using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct DeathSfx : IComponentData
    {
        public FixedString32Bytes Value;
    }
}