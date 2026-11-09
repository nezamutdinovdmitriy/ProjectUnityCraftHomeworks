using System.Collections.Generic;
using Game.Presentation.ParticleEffects;
using Game.Presentation.ParticleEffects.Ecs;
using Unity.Entities;

namespace Game.Components
{
    public sealed class ParticleEffectsState : IComponentData
    {
        public ParticleEffectService Service;
        public readonly List<ParticleEffectFollowBinding> Followers = new();
    }
}