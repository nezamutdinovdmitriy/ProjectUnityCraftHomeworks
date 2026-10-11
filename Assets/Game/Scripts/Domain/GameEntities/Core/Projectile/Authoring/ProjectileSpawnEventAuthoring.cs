using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public class ProjectileSpawnEventAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        
        private sealed class Baker : Baker<ProjectileSpawnEventAuthoring>
        {
            public override void Bake(ProjectileSpawnEventAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ProjectileSpawnEvent());
            }
        }
    }
}