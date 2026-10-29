using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class UnitPurchaseAIAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<UnitPurchaseAIAuthoring>
        {
            public override void Bake(UnitPurchaseAIAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new UnitPurchaseAI
                {
                    SelectedItemIndex = -1
                });
            }
        }
    }
}