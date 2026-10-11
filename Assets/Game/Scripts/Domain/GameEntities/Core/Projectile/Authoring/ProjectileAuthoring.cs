using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ProjectileAuthoring : MonoBehaviour
    {
        private sealed class Baker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Projectile());
            }
        }
    }
}