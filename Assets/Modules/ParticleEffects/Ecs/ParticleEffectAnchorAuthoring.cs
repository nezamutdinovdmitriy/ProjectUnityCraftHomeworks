using Unity.Entities;
using UnityEngine;

namespace Game.Presentation.ParticleEffects.Ecs
{
    [DisallowMultipleComponent]
    public sealed class ParticleEffectAnchorAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform _effectAnchor;
        
        private sealed class Baker : Baker<ParticleEffectAnchorAuthoring>
        {
            public override void Bake(ParticleEffectAnchorAuthoring authoring)
            {
                DependsOn(authoring.transform);
                DependsOn(authoring._effectAnchor);
                
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                Vector3 localPosition = authoring.transform.InverseTransformPoint(
                    authoring._effectAnchor.position);
                
                AddComponent(entity, new ParticleEffectAnchor
                {
                    Value = localPosition
                });
            }
        }
    }
}