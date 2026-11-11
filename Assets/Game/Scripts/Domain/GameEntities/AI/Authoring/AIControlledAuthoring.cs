using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class AIControlledAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<AIControlledAuthoring>
        {
            public override void Bake(AIControlledAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new AIControlled());
            }
        }
    }
}