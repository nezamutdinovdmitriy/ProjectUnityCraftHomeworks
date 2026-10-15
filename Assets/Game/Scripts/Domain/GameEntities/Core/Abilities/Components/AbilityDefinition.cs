using Unity.Entities;

namespace Game.Components
{
    public struct AbilityDefinition : IComponentData
    {
        public float Range;
        public float Windup;
        public float Cooldown;
    }
}