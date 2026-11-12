using Game.Components;
using Game.View.Vfx;
using Modules.ParticleEffects;
using Unity.Entities;
using Unity.Transforms;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class TakeDamageParticleEffectSystem : SystemBase
    {
        private BufferLookup<TakeDamageEvent> _eventsLookup;

        protected override void OnCreate()
        {
            RequireForUpdate<ParticleEffectAnchor>();
            _eventsLookup = GetBufferLookup<TakeDamageEvent>(true);
        }

        protected override void OnUpdate()
        {
            ParticleEffectService<ParticleEffectId> particleService =
                GameParticleEffectService.Instance;

            if (particleService == null)
                return;

            Dependency.Complete();
            _eventsLookup.Update(this);

            foreach (var (
                         model,
                         anchor,
                         view) in SystemAPI.Query<
                             RefRO<ModelLink>,
                             RefRO<ParticleEffectAnchor>>()
                         .WithAll<LocalToWorld>()
                         .WithEntityAccess())
            {
                if (!_eventsLookup.TryGetBuffer(
                        model.ValueRO.Value,
                        out DynamicBuffer<TakeDamageEvent> events))
                    continue;

                ParticleEffectTarget target = null;

                foreach (TakeDamageEvent takeDamageEvent in events)
                {
                    if(takeDamageEvent.Damage <= 0f)
                        continue;

                    target ??= new ParticleEffectTarget(World, view, anchor.ValueRO.Value);
                    particleService.PlayFollowing(ParticleEffectId.TakeDamage, target);
                }
            }
        }
    }
}