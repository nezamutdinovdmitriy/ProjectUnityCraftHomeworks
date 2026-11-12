using Modules.ParticleEffects;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.View.Vfx
{
    public class ParticleEffectTarget : IParticleEffectTarget
    {
        private readonly World _world;
        private readonly Entity _model;
        private readonly float3 _localOffset;

        public ParticleEffectTarget(World world, Entity model, float3 localOffset)
        {
            _world = world;
            _model = model;
            _localOffset = localOffset;
        }

        public bool TryGetPosition(out Vector3 position)
        {
            position = default;

            if (!_world.IsCreated)
                return false;

            EntityManager manager = _world.EntityManager;
            if (!manager.Exists(_model) || !manager.HasComponent<LocalToWorld>(_model))
                return false;

            LocalToWorld transform = manager.GetComponentData<LocalToWorld>(_model);
            position = math.transform(transform.Value, _localOffset);
            return true;
        }
    }
}