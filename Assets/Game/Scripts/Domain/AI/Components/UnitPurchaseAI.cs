using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct UnitPurchaseAI : IComponentData
    {
        public Random Generator;
        public int SelectedItemIndex;
    }
}