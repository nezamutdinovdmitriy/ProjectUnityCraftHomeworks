using Unity.Entities;

namespace Game.Components
{
    public struct GameOverRequest : IComponentData, IEnableableComponent
    {
        public GameStateType Value;
    }
}