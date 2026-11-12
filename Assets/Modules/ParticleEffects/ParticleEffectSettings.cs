using System;
using UnityEngine;

namespace Modules.ParticleEffects
{
    [Serializable]
    public class ParticleEffectSettings<TKey> where TKey : struct, Enum
    {
        [SerializeField] private TKey _id;
        [SerializeField] private ParticleEffectView _prefab;
        [SerializeField] private Vector3 _prefabOffset;
        
        [SerializeField, Min(0)]
        private int _prewarmCount = 4;

        [SerializeField, Min(1)]
        private int _maxRetained = 16;

        public TKey Id => _id;
        public ParticleEffectView Prefab => _prefab;
        public Vector3 PrefabOffset => _prefabOffset;
        public int PrewarmCount => _prewarmCount;
        public int MaxRetained => _maxRetained;
    }
}