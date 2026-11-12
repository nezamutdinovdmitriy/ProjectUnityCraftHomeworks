using System;
using System.Collections.Generic;
using Modules.ParticleEffects;
using UnityEngine;

namespace Game.View.Vfx
{
    [DisallowMultipleComponent]
    public sealed class GameParticleEffectService : ParticleEffectService<ParticleEffectId>
    {
        [SerializeField]
        private GameParticleEffectSettings[] _effects =
            Array.Empty<GameParticleEffectSettings>();

        protected override IReadOnlyList<ParticleEffectSettings<ParticleEffectId>> Settings => _effects;
    }
}