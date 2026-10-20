using UnityEngine;

namespace Game.Presentation.ParticleEffects
{
    public readonly struct ParticleEffectHandle
    {
        private readonly ParticleEffectService _owner;
        private readonly ulong _playbackId;

        public ParticleEffectHandle(ParticleEffectService owner, ulong playbackId)
        {
            _owner = owner;
            _playbackId = playbackId;
        }

        public bool IsPlaying 
            => _owner != null && _owner.IsPlaying(_playbackId);

        public bool TrySetPosition(Vector3 anchorPosition) 
            => _owner != null && _owner.TrySetPosition(_playbackId, anchorPosition);

        public void Stop()
        {
            if (_owner != null)
                _owner.Stop(_playbackId);
        }
    }
}