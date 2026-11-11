using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct EnemyAIState : IComponentData
    {
        public Random Generator;
        public int SelectedItemIndex;
    }
}