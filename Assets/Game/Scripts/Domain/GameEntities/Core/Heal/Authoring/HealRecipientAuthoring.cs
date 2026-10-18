using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class HealRecipientAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<HealRecipientAuthoring>
        {
            public override void Bake(HealRecipientAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddBuffer<HealRequest>(entity);
                AddBuffer<HealEvent>(entity);
            }
        }
    }
}