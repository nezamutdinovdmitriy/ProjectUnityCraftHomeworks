using Game.Components;
using Game.Presentation.Audio;
using Game.UseCases;
using Modules.AudioEvents;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class AbilitySoundsSystem : SystemBase
    {
        private ComponentLookup<AbilityStartedEvent> _startedLookup;
        private ComponentLookup<AbilityExecutedEvent> _executedLookup;
        private ComponentLookup<AbilitySounds> _soundsLookup;

        private readonly AudioEventKeyCache _keys = new();

        protected override void OnCreate()
        {
            RequireForUpdate<ModelLink>();

            _startedLookup = GetComponentLookup<AbilityStartedEvent>(true);
            _executedLookup = GetComponentLookup<AbilityExecutedEvent>(true);
            _soundsLookup = GetComponentLookup<AbilitySounds>(true);
        }

        protected override void OnUpdate()
        {
            AudioSystem audio = AudioSystem.Instance;

            if (audio == null || audio.isActiveAndEnabled == false)
                return;

            Dependency.Complete();

            _startedLookup.Update(this);
            _executedLookup.Update(this);
            _soundsLookup.Update(this);

            foreach (var (
                         modelLink,
                         transform)
                     in SystemAPI.Query<
                         RefRO<ModelLink>,
                         RefRO<LocalToWorld>>())
            {
                Entity actor = modelLink.ValueRO.Value;
                Vector3 position = transform.ValueRO.Position;

                if (_startedLookup.HasComponent(actor) 
                    && _startedLookup.IsComponentEnabled(actor))
                {
                    Entity definition = _startedLookup.GetRefRO(actor).ValueRO.Definition;

                    if (_soundsLookup.TryGetRefRO(definition, out RefRO<AbilitySounds> sounds))
                        AudioUseCase.Play(audio, sounds.ValueRO.Started, position, _keys);
                }

                if (_executedLookup.HasComponent(actor) 
                    && _executedLookup.IsComponentEnabled(actor))
                {
                    Entity definition = _executedLookup.GetRefRO(actor).ValueRO.Definition;

                    if (_soundsLookup.TryGetRefRO(definition, out RefRO<AbilitySounds> sounds))
                        AudioUseCase.Play(audio, sounds.ValueRO.Executed, position, _keys);
                }
            }
        }
    }
}