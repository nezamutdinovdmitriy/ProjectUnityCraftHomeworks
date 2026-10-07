using Unity.Entities;

namespace Game.Components
{
    public struct AttackHitEvent : IComponentData, IEnableableComponent
    {
        public Entity Target;
    }
}