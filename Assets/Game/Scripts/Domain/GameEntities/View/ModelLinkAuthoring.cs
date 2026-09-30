using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public class ModelLinkAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _model;
        
        public class Baker : Baker<ModelLinkAuthoring>
        {
            public override void Bake(ModelLinkAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ModelLink
                {
                    Value = GetEntity(authoring._model, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}