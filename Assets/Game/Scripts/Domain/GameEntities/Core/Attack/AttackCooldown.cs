using Unity.Entities;

namespace Game.Components
{
    public struct AttackCooldown : IComponentData
    {
        public float Time;
        public float Duration;
    }
}