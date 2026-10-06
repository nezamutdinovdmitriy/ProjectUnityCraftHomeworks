using Unity.Entities;

namespace Game.Components
{
    public struct AttackRequest : IComponentData, IEnableableComponent
    {
        public Entity Target;
    }
}