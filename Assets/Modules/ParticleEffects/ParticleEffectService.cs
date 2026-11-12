using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.ParticleEffects
{
    public abstract class ParticleEffectService<TKey> : MonoBehaviour where TKey : struct, Enum
    {
        private readonly struct RegisteredEffect
        {
            public readonly ParticleEffectPool<TKey> Pool;
            public readonly Vector3 Offset;

            public RegisteredEffect(ParticleEffectPool<TKey> pool, Vector3 offset)
            {
                Pool = pool;
                Offset = offset;
            }
        }

        private sealed class ActiveEffect
        {
            public readonly RegisteredEffect Effect;
            public readonly ParticleEffectView View;
            public IParticleEffectTarget Target;

            public ActiveEffect(
                RegisteredEffect effect,
                ParticleEffectView view,
                IParticleEffectTarget target)
            {
                Effect = effect;
                View = view;
                Target = target;
            }
        }

        private sealed class TransformTarget : IParticleEffectTarget
        {
            private TransformHandle _handle;

            public TransformTarget(TransformHandle handle)
            {
                _handle = handle;
            }

            public bool TryGetPosition(out Vector3 position)
            {
                if (!_handle.IsValid())
                {
                    position = default;
                    return false;
                }

                position = _handle.position;
                return true;
            }
        }

        public static ParticleEffectService<TKey> Instance { get; private set; }

        protected abstract IReadOnlyList<ParticleEffectSettings<TKey>> Settings { get; }

        private readonly Dictionary<TKey, RegisteredEffect> _registered = new();
        private readonly Dictionary<ulong, ActiveEffect> _active = new();
        private readonly List<ulong> _finished = new();
        private ulong _nextPlaybackId;

        private bool _initialized;

        protected void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log($"Another particle effect service for {typeof(TKey).Name} is active.", this);
                enabled = false;
                return;
            }
            
            if(!_initialized)
                InitializeEffects();

            Instance = this;
        }
        
        private void LateUpdate()
        {
            foreach (KeyValuePair<ulong, ActiveEffect> pair in _active)
            {
                ActiveEffect active = pair.Value;

                if (active.View == null ||
                    !active.View.IsAlive())
                {
                    _finished.Add(pair.Key);
                    continue;
                }
                    
                if(active.Target == null)
                    continue;
                
                if(active.Target.TryGetPosition(out Vector3 position))
                    active.View.SetPosition(position + active.Effect.Offset);
                else
                    active.Target = null;
            }

            foreach (ulong playbackId in _finished)
                Stop(playbackId);

            _finished.Clear();
        }

        protected void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            _finished.Clear();
            _finished.AddRange(_active.Keys);

            foreach (ulong playbackId in _finished)
                Stop(playbackId);

            _finished.Clear();
        }

        private void OnDestroy()
        {
            foreach (RegisteredEffect effect in _registered.Values)
                effect.Pool.Dispose();

            _registered.Clear();
        }
        
        public bool IsPlaying(ulong playbackId) => _active.ContainsKey(playbackId);
        
        public ParticleEffectHandle<TKey> Play(TKey key, Vector3 position)
        {
            return PlayInternal(key, position, null);
        }
        
        public ParticleEffectHandle<TKey> PlayFollowing(TKey key, TransformHandle target)
        {
            return PlayFollowing(key, new TransformTarget(target));
        }

        public ParticleEffectHandle<TKey> PlayFollowing(TKey key, IParticleEffectTarget target)
        {
            if (target == null || !target.TryGetPosition(out Vector3 position))
                return default;

            return PlayInternal(key, position, target);
        }

        private ParticleEffectHandle<TKey> PlayInternal(
            TKey key,
            Vector3 position,
            IParticleEffectTarget target)
        {
            if (!isActiveAndEnabled || !_registered.TryGetValue(key, out RegisteredEffect effect))
                return default;

            ParticleEffectView view = effect.Pool.Rent();
            view.PlayAt(position + effect.Offset);

            ulong playbackId = ++_nextPlaybackId;
            _active.Add(playbackId, new ActiveEffect(effect, view, target));

            return new ParticleEffectHandle<TKey>(this, playbackId);
        }
        
        public void Stop(ulong playbackId)
        {
            if (_active.Remove(playbackId, out ActiveEffect active) &&
                active.View != null)
                active.Effect.Pool.Release(active.View);
        }
        
        private void InitializeEffects()
        {
            foreach (ParticleEffectSettings<TKey> settings in Settings)
            {
                if (settings == null ||
                    settings.Prefab == null ||
                    EqualityComparer<TKey>.Default.Equals(settings.Id, default) ||
                    _registered.ContainsKey(settings.Id))
                    continue;

                ParticleEffectPool<TKey> pool = new ParticleEffectPool<TKey>(transform, settings);

                _registered.Add(settings.Id, new RegisteredEffect(pool, settings.PrefabOffset));
            }

            _initialized = true;
        }
    }
}