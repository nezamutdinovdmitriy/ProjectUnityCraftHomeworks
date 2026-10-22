using Unity.Entities;

namespace Game.Components
{
    public struct AbilityExecutedEvent : IComponentData, IEnableableComponent
    {
        public Entity Definition;
        public Entity Target;
    }
}