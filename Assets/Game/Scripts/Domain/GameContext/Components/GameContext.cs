using Unity.Entities;

namespace Game.Components
{
    public struct GameContext : IComponentData
    {
        public GameResult Result;
    }
}