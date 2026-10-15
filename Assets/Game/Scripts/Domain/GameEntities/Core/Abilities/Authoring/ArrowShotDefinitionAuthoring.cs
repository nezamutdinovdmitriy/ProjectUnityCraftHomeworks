using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class ArrowShotDefinitionAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        
        private sealed class Baker : Baker<ArrowShotDefinitionAuthoring>
        {
            public override void Bake(ArrowShotDefinitionAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                Entity projectilePrefab = authoring._projectilePrefab != null
                    ? GetEntity(authoring._projectilePrefab, TransformUsageFlags.Dynamic)
                    : Entity.Null;
                
                AddComponent(entity, new ArrowShotAbility
                {
                    ProjectilePrefab = projectilePrefab
                });
            }
        }
    }
}