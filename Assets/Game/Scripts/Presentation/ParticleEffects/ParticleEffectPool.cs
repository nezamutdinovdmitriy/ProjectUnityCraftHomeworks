using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Presentation.ParticleEffects
{
    internal sealed class ParticleEffectPool : IDisposable
    {
        private readonly ParticleEffectView _prefab;
        private readonly Transform _storage;
        private readonly ObjectPool<ParticleEffectView> _pool;

        public ParticleEffectPool(Transform root, ParticleEffectSettings settings)
        {
            _prefab = settings.Prefab;

            GameObject storage = new GameObject($"{settings.Id} (Pool)");

            storage.transform.SetParent(root, worldPositionStays: false);

            _storage = storage.transform;

            int maxRetained = Mathf.Max(1, settings.MaxRetained);
            int prewarmCount = Mathf.Clamp(settings.PrewarmCount, 0, maxRetained);

            _pool = new ObjectPool<ParticleEffectView>(
                createFunc: CreateInstance,
                actionOnGet: null,
                actionOnRelease: ReturnToStorage,
                actionOnDestroy: DestroyInstance,
                collectionCheck: true,
                defaultCapacity: Math.Max(1, prewarmCount),
                maxSize: maxRetained);

            ParticleEffectView[] instances = new ParticleEffectView[prewarmCount];

            for (int i = 0; i < instances.Length; i++)
                instances[i] = _pool.Get();

            foreach (ParticleEffectView instance in instances)
                _pool.Release(instance);
        }

        public ParticleEffectView Rent() 
            => _pool.Get();

        public void Release(ParticleEffectView instance) 
            => _pool.Release(instance);

        public void Dispose()
        {
            _pool.Clear();

            if (_storage != null)
                Object.Destroy(_storage.gameObject);
        }

        private ParticleEffectView CreateInstance()
        {
            ParticleEffectView instance = Object.Instantiate(_prefab, _storage, false);

            instance.gameObject.SetActive(false);
            instance.Initialize();
            instance.StopAndClear();

            return instance;
        }

        private void ReturnToStorage(ParticleEffectView instance)
        {
            instance.StopAndClear();
            instance.gameObject.SetActive(false);
        }

        private static void DestroyInstance(ParticleEffectView instance)
        {
            if (instance != null)
                Object.Destroy(instance.gameObject);
        }
    }
}