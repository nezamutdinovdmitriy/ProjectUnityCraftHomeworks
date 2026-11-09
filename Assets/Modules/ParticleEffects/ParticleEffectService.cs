using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Presentation.ParticleEffects
{
    [DisallowMultipleComponent]
    public sealed class ParticleEffectService : MonoBehaviour
    {
        private readonly struct RegisteredEffect
        {
            public readonly ParticleEffectPool Pool;
            public readonly Vector3 Offset;

            public RegisteredEffect(ParticleEffectPool pool, Vector3 offset)
            {
                Pool = pool;
                Offset = offset;
            }
        }

        private readonly struct ActiveEffect
        {
            public readonly ParticleEffectPool Pool;
            public readonly ParticleEffectView Instance;
            public readonly Vector3 Offset;

            public ActiveEffect(ParticleEffectPool pool, ParticleEffectView instance, Vector3 offset)
            {
                Pool = pool;
                Instance = instance;
                Offset = offset;
            }
        }
        
        [SerializeField] private ParticleEffectSettings[] _effects = Array.Empty<ParticleEffectSettings>();

        private Dictionary<ParticleEffectId, RegisteredEffect> _registered;
        private readonly Dictionary<ulong, ActiveEffect> _active = new();
        private readonly List<ulong> _finished = new();
        private ulong _nextPlaybackId;

        private void Awake() => InitializeEffects();

        public ParticleEffectHandle Play(ParticleEffectId effectId, Vector3 anchorPosition)
        {
            if (isActiveAndEnabled == false)
                return default;

            InitializeEffects();

            if (_registered.TryGetValue(effectId, out RegisteredEffect effect) == false)
                return default;

            ParticleEffectView instance = effect.Pool.Rent();
            instance.PlayAt(anchorPosition + effect.Offset);

            ulong playbackId = ++_nextPlaybackId;
            
            _active.Add(playbackId, new ActiveEffect(effect.Pool, instance, effect.Offset));

            return new ParticleEffectHandle(this, playbackId);
        }

        private void InitializeEffects()
        {
            if (_registered != null)
                return;

            _registered = new Dictionary<ParticleEffectId, RegisteredEffect>();

            foreach (ParticleEffectSettings settings in _effects)
            {
                if (settings == null 
                    || settings.Id == ParticleEffectId.None 
                    || settings.Prefab == null
                    || _registered.ContainsKey(settings.Id))
                    continue;

                ParticleEffectPool pool = new ParticleEffectPool(transform, settings);
                
                _registered.Add(settings.Id, new RegisteredEffect(pool, settings.PrefabOffset));
            }
        }

        private void LateUpdate()
        {
            _finished.Clear();

            foreach (var pair in _active)
            {
                if (pair.Value.Instance == null 
                    || pair.Value.Instance.IsAlive() == false)
                    _finished.Add(pair.Key);
            }

            foreach (ulong playbackId in _finished)
                Stop(playbackId);

            _finished.Clear();
        }

        internal bool IsPlaying(ulong playbackId) 
            => _active.ContainsKey(playbackId);

        internal bool TrySetPosition(ulong playbackId, Vector3 anchorPosition)
        {
            if (_active.TryGetValue(playbackId, out ActiveEffect effect) == false
                || effect.Instance == null)
                return false;

            effect.Instance.transform.position = anchorPosition + effect.Offset;
            
            return true;
        }

        internal void Stop(ulong playbackId)
        {
            if (_active.Remove(playbackId, out ActiveEffect effect) == false)
                return;

            if (effect.Instance != null)
                effect.Pool.Release(effect.Instance);
        }

        private void OnDisable() => StopAll();

        private void OnDestroy()
        {
            StopAll();

            if (_registered == null)
                return;

            foreach (RegisteredEffect effect in _registered.Values)
                effect.Pool.Dispose();

            _registered.Clear();
            _registered = null;
        }

        private void StopAll()
        {
            _finished.Clear();
            _finished.AddRange(_active.Keys);

            foreach (ulong playbackId in _finished)
                Stop(playbackId);

            _finished.Clear();
        }
    }
}