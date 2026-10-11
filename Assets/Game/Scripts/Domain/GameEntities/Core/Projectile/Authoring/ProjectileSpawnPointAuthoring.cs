using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ProjectileSpawnPointAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform _attackPoint;
        
        private sealed class Baker : Baker<ProjectileSpawnPointAuthoring>
        {
            public override void Bake(ProjectileSpawnPointAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ProjectileSpawnPoint
                {
                    Value = GetEntity(authoring._attackPoint, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}