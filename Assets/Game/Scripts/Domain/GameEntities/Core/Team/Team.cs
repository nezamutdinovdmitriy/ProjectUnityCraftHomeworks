using Unity.Entities;

namespace Game.Components
{
    public struct Team : IComponentData
    {
        public TeamType Value;
    }
}