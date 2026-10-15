using Unity.Entities;

namespace Game.Components
{
    public struct SelectedAbility : IComponentData
    {
        public Entity Value;
    }
}