using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public sealed class ArcherAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<ArcherAuthoring>
        {
            public override void Bake(ArcherAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Archer());
            }
        }
    }
}