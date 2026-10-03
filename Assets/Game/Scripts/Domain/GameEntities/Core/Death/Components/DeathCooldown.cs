using Unity.Entities;

namespace Game.Components
{
    public struct DeathCooldown : IComponentData, IEnableableComponent
    {
        public float Time;
        public float Duration;
    }
}