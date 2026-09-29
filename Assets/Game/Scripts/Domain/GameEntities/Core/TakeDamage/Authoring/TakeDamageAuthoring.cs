using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class TakeDamageAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<TakeDamageAuthoring>
        {
            public override void Bake(TakeDamageAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddBuffer<TakeDamageRequest>(entity);
                AddBuffer<TakeDamageEvent>(entity);
            }
        }
    }
}