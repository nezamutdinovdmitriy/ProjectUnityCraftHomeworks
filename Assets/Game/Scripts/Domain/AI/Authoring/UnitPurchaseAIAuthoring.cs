using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class UnitPurchaseAIAuthoring : MonoBehaviour
    {
        [SerializeField] private uint _seed = 1;
        
        private sealed class Baker : Baker<UnitPurchaseAIAuthoring>
        {
            public override void Bake(UnitPurchaseAIAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new UnitPurchaseAI
                {
                    Generator = new Random(authoring._seed),
                    SelectedItemIndex = -1
                });
            }
        }
    }
}