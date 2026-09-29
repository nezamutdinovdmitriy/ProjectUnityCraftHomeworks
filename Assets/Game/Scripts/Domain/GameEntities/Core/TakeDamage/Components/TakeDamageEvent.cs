using Unity.Entities;

namespace Game.Components
{
    [InternalBufferCapacity(5)]
    public struct TakeDamageEvent : IBufferElementData
    {
        public float Damage;
    }
}