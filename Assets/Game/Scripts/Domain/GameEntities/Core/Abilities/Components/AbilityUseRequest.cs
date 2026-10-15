using Unity.Entities;

namespace Game.Components
{
    public struct AbilityUseRequest : IComponentData, IEnableableComponent
    {
        public Entity Definition;
        public Entity Target;

        public bool Preparing;
        public bool CancelRequested;
    }
}