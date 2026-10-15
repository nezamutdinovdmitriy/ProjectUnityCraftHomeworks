using Unity.Entities;

namespace Game.Components
{
    public struct AbilityCooldown : IComponentData, IEnableableComponent
    {
        public float Time;
    }
}