using Unity.Entities;
using UnityEngine;

namespace Game.Components
{
    public sealed class DefaultTargetEntityAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _defaultTarget;
        
        private sealed class Baker: Baker<DefaultTargetEntityAuthoring>
        {
            public override void Bake(DefaultTargetEntityAuthoring authoring)
            {
                if (authoring._defaultTarget != null)
                {
                    AddComponent(GetEntity(TransformUsageFlags.None), new DefaultTargetEntity
                    {
                        Value = GetEntity(authoring._defaultTarget, TransformUsageFlags.None)
                    });
                }
                else
                {
                    AddComponent(GetEntity(TransformUsageFlags.None), new DefaultTargetEntity());
                }
                
            }
        }
    }
}