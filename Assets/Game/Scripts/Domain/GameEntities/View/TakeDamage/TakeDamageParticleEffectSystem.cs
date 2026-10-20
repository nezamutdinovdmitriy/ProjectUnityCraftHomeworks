using Game.Components;
using Game.Presentation.ParticleEffects;
using Game.Presentation.ParticleEffects.Ecs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class TakeDamageParticleEffectSystem : SystemBase
    {
        private BufferLookup<TakeDamageEvent> _eventsLookup;

        protected override void OnCreate()
        {
            _eventsLookup = GetBufferLookup<TakeDamageEvent>(true);
        }

        protected override void OnUpdate()
        {
            if (SystemAPI.ManagedAPI.TryGetSingleton(
                    out ParticleEffectsState state) == false
                || state.Service == null
                || state.Service.isActiveAndEnabled == false)
                return;

            Dependency.Complete();
            _eventsLookup.Update(this);

            foreach (var (
                         model,
                         anchor,
                         transform,
                         view) in SystemAPI.Query<
                             RefRO<ModelLink>,
                             RefRO<ParticleEffectAnchor>,
                             RefRO<LocalToWorld>>()
                         .WithEntityAccess())
            {
                Entity eventOwner = model.ValueRO.Value;

                if (_eventsLookup.TryGetBuffer(
                        eventOwner, out DynamicBuffer<TakeDamageEvent> events) == false
                    || events.IsEmpty)
                    continue;
                
                float3 anchorPosition = math.transform(
                    transform.ValueRO.Value,
                    anchor.ValueRO.Value);
                
                foreach (TakeDamageEvent damageEvent in events)
                {
                    if (damageEvent.Damage <= 0f)
                        continue;

                    ParticleEffectHandle handle = state.Service.Play(
                        ParticleEffectId.TakeDamage,
                        new Vector3(anchorPosition.x, anchorPosition.y, anchorPosition.z));

                    if (handle.IsPlaying)
                    {
                        state.Followers.Add(
                            new ParticleEffectFollowBinding
                            {
                                Target = view,
                                Handle = handle
                            });
                    }
                }
            }
        }
    }
}