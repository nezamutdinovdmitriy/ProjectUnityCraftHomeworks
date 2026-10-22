using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct AbilitySounds : IComponentData
    {
        public FixedString128Bytes Started;
        public FixedString128Bytes Executed;
    }
}