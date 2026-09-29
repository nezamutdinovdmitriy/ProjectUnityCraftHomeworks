using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public class EntityNameAuthoring : MonoBehaviour
    {
        [SerializeField] private string _entityName;
        
        public class Baker : Baker<EntityNameAuthoring>
        {
            public override void Bake(EntityNameAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EntityName {Value = authoring._entityName});
            }
        }
    }
}