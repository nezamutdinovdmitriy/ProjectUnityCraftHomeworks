using Unity.Entities;

namespace Game.Components
{
    public struct AbilityWindup : IComponentData, IEnableableComponent
    {
        public float Time;
    }
}