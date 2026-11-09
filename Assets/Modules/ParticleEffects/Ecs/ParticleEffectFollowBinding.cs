using Game.View;
using Unity.Entities;

namespace Game.Presentation.ParticleEffects.Ecs
{
    public struct ParticleEffectFollowBinding
    {
        public Entity Target;
        public ParticleEffectHandle Handle;
    }
}