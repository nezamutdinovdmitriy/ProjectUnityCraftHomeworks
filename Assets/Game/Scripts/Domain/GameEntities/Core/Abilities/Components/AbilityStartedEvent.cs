using Unity.Entities;

namespace Game.Components
{
    public struct AbilityStartedEvent : IComponentData, IEnableableComponent
    {
        public Entity Definition;
    }
}