using Unity.Entities;

namespace Game.Components
{
    public struct AmmoRestoreCooldown : IComponentData
    {
        public float Time;
        public float Cooldown;
    }
}