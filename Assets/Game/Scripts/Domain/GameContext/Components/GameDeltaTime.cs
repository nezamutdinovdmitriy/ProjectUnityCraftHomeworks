using Unity.Entities;

namespace Game.Components
{
    public struct GameDeltaTime : IComponentData
    {
        public float DeltaTime;
    }
}