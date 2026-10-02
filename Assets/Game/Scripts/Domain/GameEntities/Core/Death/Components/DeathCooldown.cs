using Unity.Entities;

namespace Game.Scripts.Domain.GameEntities.Core.Death
{
    public struct DeathCooldown : IComponentData
    {
        public float Time;
        public float Duration;
    }
}