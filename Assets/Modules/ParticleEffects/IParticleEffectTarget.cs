using UnityEngine;

namespace Modules.ParticleEffects
{
    public interface IParticleEffectTarget
    {
        bool TryGetPosition(out Vector3 position);
    }
}