using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class DetectRadiusAuthoring : MonoBehaviour
    {
        [SerializeField] private float _detectRadius;
        
        private sealed class Baker : Baker<DetectRadiusAuthoring>
        {
            public override void Bake(DetectRadiusAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new DetectRadius {Value = authoring._detectRadius});
            }
        }
    }
}