using Unity.Entities;

namespace Game.Components
{
    public struct TargetEntity : IComponentData
    {
        public Entity Value;
    }
}