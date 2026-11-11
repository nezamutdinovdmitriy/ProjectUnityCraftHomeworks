using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class EnemyAIAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<EnemyAIAuthoring>
        {
            public override void Bake(EnemyAIAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new EnemyAIState
                {
                    SelectedItemIndex = -1
                });
            }
        }
    }
}