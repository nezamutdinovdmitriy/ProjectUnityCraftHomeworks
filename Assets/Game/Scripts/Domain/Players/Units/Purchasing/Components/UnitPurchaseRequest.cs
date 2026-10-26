using Unity.Entities;

namespace Game.Components
{
    public struct UnitPurchaseRequest : IBufferElementData
    {
        public int ItemIndex;
    }
}