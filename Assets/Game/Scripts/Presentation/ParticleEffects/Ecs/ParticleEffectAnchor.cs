using Unity.Entities;
using Unity.Mathematics;

namespace Game.Presentation.ParticleEffects.Ecs
{
    public struct ParticleEffectAnchor : IComponentData
    {
        public float3 Value;
    }
}