using Game.Components;
using Game.View;
using Game.View.Vfx;
using Modules.ParticleEffects;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class HealParticleEffectSystem : SystemBase
    {
        private BufferLookup<HealEvent> _eventsLookup;

        protected override void OnCreate()
        {
            RequireForUpdate<ParticleEffectAnchor>();
            _eventsLookup = GetBufferLookup<HealEvent>(isReadOnly: true);
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
                         view)
                     in SystemAPI.Query<RefRO<ModelLink>, RefRO<ParticleEffectAnchor>>()
                         .WithAll<LocalToWorld>()
                         .WithEntityAccess())
            {
                if(!_eventsLookup.TryGetBuffer(
                       model.ValueRO.Value,
                       out DynamicBuffer<HealEvent> events))
                    continue;

                ParticleEffectTarget target = null;

                foreach (HealEvent healEvent in events)
                {
                    if (healEvent.Value <= 0f)
                        continue;

                    target ??= new ParticleEffectTarget(World, view, anchor.ValueRO.Value);
                    particleService.PlayFollowing(ParticleEffectId.Heal, target);
                }
            }
        }
    }
}