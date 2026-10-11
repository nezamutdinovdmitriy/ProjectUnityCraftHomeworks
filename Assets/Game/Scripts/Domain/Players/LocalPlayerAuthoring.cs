using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class LocalPlayerAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<LocalPlayerAuthoring>
        {
            public override void Bake(LocalPlayerAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new LocalPlayer());
            }
        }
    }
}