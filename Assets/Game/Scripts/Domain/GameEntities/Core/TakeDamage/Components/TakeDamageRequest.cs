using Unity.Entities;

namespace Game.Components
{
    [InternalBufferCapacity(5)]
    public struct TakeDamageRequest : IBufferElementData
    {
        public float Damage;
    }
}