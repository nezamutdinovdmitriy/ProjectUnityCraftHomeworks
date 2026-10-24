using Unity.Entities;

namespace Game.Components
{
    public struct GameTime : IComponentData
    {
        public float DeltaTime;
    }
}