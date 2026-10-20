using System.Collections.Generic;
using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.Presentation.ParticleEffects.Ecs
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class ParticleEffectFollowSystem : SystemBase
    {
        private ComponentLookup<LocalToWorld> _worldTransformLookup;
        private ComponentLookup<ParticleEffectAnchor> _anchorLookup;

        protected override void OnCreate()
        {
            _worldTransformLookup = GetComponentLookup<LocalToWorld>(isReadOnly: true);
            _anchorLookup = GetComponentLookup<ParticleEffectAnchor>(true);
        }

        protected override void OnUpdate()
        {
            if (SystemAPI.ManagedAPI.TryGetSingleton(
                    out ParticleEffectsState state) == false)
                return;

            Dependency.Complete();

            _worldTransformLookup.Update(this);
            _anchorLookup.Update(this);

            List<ParticleEffectFollowBinding> followers = state.Followers;

            for (int i = followers.Count - 1; i >= 0; i--)
            {
                ParticleEffectFollowBinding binding = followers[i];

                if (binding.Handle.IsPlaying == false
                    || _worldTransformLookup.TryGetRefRO(
                        binding.Target,
                        out RefRO<LocalToWorld> targetTransform) == false)
                {
                    int last = followers.Count - 1;
                    followers[i] = followers[last];
                    followers.RemoveAt(last);
                    continue;
                }

                float3 anchorPosition = targetTransform.ValueRO.Position;

                if (_anchorLookup.TryGetRefRO(
                        binding.Target, out RefRO<ParticleEffectAnchor> anchor))
                {
                    anchorPosition = math.transform(
                        targetTransform.ValueRO.Value,
                        anchor.ValueRO.Value);
                }

                binding.Handle.TrySetPosition(
                    new Vector3(anchorPosition.x, anchorPosition.y, anchorPosition.z));
            }
        }
    }
}