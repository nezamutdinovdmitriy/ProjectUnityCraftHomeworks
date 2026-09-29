using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public class ModelEntityAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _model;
        
        public class ModelEntityBaker : Baker<ModelEntityAuthoring>
        {
            public override void Bake(ModelEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ModelEntity
                {
                    Value = GetEntity(authoring._model, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}