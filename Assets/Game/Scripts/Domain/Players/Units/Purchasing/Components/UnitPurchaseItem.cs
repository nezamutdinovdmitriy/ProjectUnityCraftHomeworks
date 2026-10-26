using Unity.Entities;

namespace Game.Components
{
    public struct UnitPurchaseItem : IBufferElementData
    {
        public Entity Prefab;
        public int Price;
    }
}