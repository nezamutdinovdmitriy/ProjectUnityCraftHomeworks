using Game.Components;
using Game.Presentation.Audio;
using Game.UseCases;
using Modules.AudioEvents;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class UnitSoundsSystem : SystemBase
    {
        private BufferLookup<TakeDamageEvent> _damageLookup;
        private BufferLookup<HealEvent> _healLookup;
        private ComponentLookup<DeathEvent> _deathLookup;
        
        private readonly AudioEventKeyCache _keys = new();

        protected override void OnCreate()
        {
            RequireForUpdate<UnitSounds>();

            _damageLookup = GetBufferLookup<TakeDamageEvent>(true);
            _healLookup = GetBufferLookup<HealEvent>(true);
            _deathLookup = GetComponentLookup<DeathEvent>(true);
        }

        protected override void OnUpdate()
        {
            AudioSystem audio = AudioSystem.Instance;

            if (audio == null || !audio.isActiveAndEnabled)
                return;

            Dependency.Complete();

            _damageLookup.Update(this);
            _healLookup.Update(this);
            _deathLookup.Update(this);

            foreach (var (sounds, modelLink, transform) in
                     SystemAPI.Query<RefRO<UnitSounds>, RefRO<ModelLink>, RefRO<LocalToWorld>>())
            {
                Entity actor = modelLink.ValueRO.Value;
                Vector3 position = transform.ValueRO.Position;

                if (_damageLookup.TryGetBuffer(actor, out DynamicBuffer<TakeDamageEvent> damageEvents))
                {
                    foreach (TakeDamageEvent damageEvent in damageEvents)
                    {
                        if (damageEvent.Damage > 0f)
                            AudioUseCase.Play(audio, sounds.ValueRO.TakeDamage, position, _keys);
                    }
                }

                if (_healLookup.TryGetBuffer(actor, out DynamicBuffer<HealEvent> healEvents))
                {
                    foreach (HealEvent healEvent in healEvents)
                    {
                        if (healEvent.Value > 0f)
                            AudioUseCase.Play(audio, sounds.ValueRO.Heal, position, _keys);
                    }
                }

                if (_deathLookup.HasComponent(actor) && _deathLookup.IsComponentEnabled(actor))
                    AudioUseCase.Play(audio, sounds.ValueRO.Death, position, _keys);
            }
        }
    }
}