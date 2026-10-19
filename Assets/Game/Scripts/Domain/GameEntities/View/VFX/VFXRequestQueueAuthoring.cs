using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class VFXRequestQueueAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<VFXRequestQueueAuthoring>
        {
            public override void Bake(VFXRequestQueueAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddBuffer<VFXSpawnRequest>(entity);
            }
        }
    }
}