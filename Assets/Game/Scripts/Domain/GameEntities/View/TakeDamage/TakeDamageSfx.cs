using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct TakeDamageSfx : IComponentData
    {
        public FixedString32Bytes Key;
    }
}