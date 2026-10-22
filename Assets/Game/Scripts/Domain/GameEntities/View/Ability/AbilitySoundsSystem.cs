using Game.Components;
using Game.Presentation.Audio;
using Game.Presentation.Audio.Ecs;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct AbilitySoundsSystem : ISystem
    {
        private ComponentLookup<AbilityStartedEvent> _startedLookup;
        private ComponentLookup<AbilityExecutedEvent> _executedLookup;
        private ComponentLookup<AbilitySounds> _soundsLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ModelLink>();

            _startedLookup = state.GetComponentLookup<AbilityStartedEvent>(true);
            _executedLookup = state.GetComponentLookup<AbilityExecutedEvent>(true);
            _soundsLookup = state.GetComponentLookup<AbilitySounds>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.ManagedAPI
                    .TryGetSingleton(out AudioPlaybackState audioState) == false
                || audioState.Playback == null)
                return;

            AudioPlayback playback = audioState.Playback;

            state.Dependency.Complete();

            _startedLookup.Update(ref state);
            _executedLookup.Update(ref state);
            _soundsLookup.Update(ref state);

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
                        playback.Play(sounds.ValueRO.Started, position);
                }

                if (_executedLookup.HasComponent(actor)
                    && _executedLookup.IsComponentEnabled(actor))
                {
                    Entity definition = _executedLookup.GetRefRO(actor).ValueRO.Definition;

                    if (_soundsLookup.TryGetRefRO(definition, out RefRO<AbilitySounds> sounds))
                        playback.Play(sounds.ValueRO.Executed, position);
                }
            }
        }
    }
}