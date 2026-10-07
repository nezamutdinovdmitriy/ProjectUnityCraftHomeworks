using Unity.Entities;

namespace Game.Components
{
    public struct AttackStartedRequest : IComponentData, IEnableableComponent
    {
        public Entity Target;
    }
}