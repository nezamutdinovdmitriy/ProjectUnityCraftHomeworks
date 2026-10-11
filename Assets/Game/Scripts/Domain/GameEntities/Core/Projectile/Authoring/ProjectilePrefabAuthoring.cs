using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ProjectilePrefabAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        
        private sealed class Baker : Baker<ProjectilePrefabAuthoring>
        {
            public override void Bake(ProjectilePrefabAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ProjectilePrefab
                {
                    Value = GetEntity(authoring._projectilePrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}