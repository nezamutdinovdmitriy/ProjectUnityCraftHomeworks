using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class WoundedAllyTargetingAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<WoundedAllyTargetingAuthoring>
        {
            public override void Bake(WoundedAllyTargetingAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new WoundedAllyTargeting());
            }
        }
    }
}