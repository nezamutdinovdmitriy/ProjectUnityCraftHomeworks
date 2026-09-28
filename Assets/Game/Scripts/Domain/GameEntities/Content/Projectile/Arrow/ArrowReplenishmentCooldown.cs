using Unity.Entities;

namespace Game.Components
{
    public struct ArrowReplenishmentCooldown : IComponentData
    {
        public float Time;
        public float Duration;
    }
}