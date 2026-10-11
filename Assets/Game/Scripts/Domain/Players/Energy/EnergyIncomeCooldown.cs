using Unity.Entities;

namespace Game.Components
{
    public struct EnergyIncomeCooldown : IComponentData
    {
        public int Income;
        public float Time;
        public float Cooldown;
    }
}