using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct EntityName : IComponentData
    {
        public FixedString32Bytes Value;
    }
}