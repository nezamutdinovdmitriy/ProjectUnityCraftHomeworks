using Unity.Collections;
using Unity.Entities;

namespace Game.Components
{
    public struct UnitSounds : IComponentData
    {
        public FixedString128Bytes TakeDamage;
        public FixedString128Bytes Heal;
        public FixedString128Bytes Death;
    }
}