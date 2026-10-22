using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct AbilitySounds : IComponentData
    {
        public FixedString32Bytes Started;
        public FixedString32Bytes Executed;
    }
}