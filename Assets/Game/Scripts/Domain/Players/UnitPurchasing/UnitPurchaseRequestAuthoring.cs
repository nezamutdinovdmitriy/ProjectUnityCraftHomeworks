using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class UnitPurchaseRequestAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<UnitPurchaseRequestAuthoring>
        {
            public override void Bake(UnitPurchaseRequestAuthoring authoring)
            {
                AddBuffer<UnitPurchaseRequest>(GetEntity(TransformUsageFlags.None));
            }
        }
    }
}