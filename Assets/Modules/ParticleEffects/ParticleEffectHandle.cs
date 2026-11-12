using System;

namespace Modules.ParticleEffects
{
    public readonly struct ParticleEffectHandle<TKey> where TKey : struct, Enum
    {
        private readonly ParticleEffectService<TKey> _owner;
        private readonly ulong _playbackId;

        public ParticleEffectHandle(ParticleEffectService<TKey> owner, ulong playbackId)
        {
            _owner = owner;
            _playbackId = playbackId;
        }

        public bool IsPlaying 
            => _owner != null && _owner.IsPlaying(_playbackId);

        public void Stop()
        {
            if (_owner != null)
                _owner.Stop(_playbackId);
        }
    }
}