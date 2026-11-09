using Unity.Entities;

namespace Game.Components
{
    public struct GameState : IComponentData
    {
        public GameStateType Value;
    }
}