using System;
using UnityEngine;

namespace Game.Presentation.ParticleEffects
{
    [Serializable]
    public class ParticleEffectSettings
    {
        [SerializeField] private ParticleEffectId _id;
        [SerializeField] private ParticleEffectView _prefab;
        [SerializeField] private Vector3 _prefabOffset;
        
        [SerializeField, Min(0)]
        private int _prewarmCount = 4;

        [SerializeField, Min(1)]
        private int _maxRetained = 16;

        public ParticleEffectId Id => _id;
        public ParticleEffectView Prefab => _prefab;
        public Vector3 PrefabOffset => _prefabOffset;
        public int PrewarmCount => _prewarmCount;
        public int MaxRetained => _maxRetained;
    }
}