using Unity.Entities;

namespace Game.Components
{
    public struct AbilityExecutedEvent : IComponentData, IEnableableComponent
    {
        public Entity Target;
    }
}