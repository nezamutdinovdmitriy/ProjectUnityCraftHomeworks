using Game.Components;
using Game.Presentation.Audio;
using Game.Presentation.Audio.Ecs;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct DeathSfxSystem : ISystem
    {
        private ComponentLookup<DeathEvent> _eventsLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DeathSfx>();

            _eventsLookup = SystemAPI.GetComponentLookup<DeathEvent>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.ManagedAPI
                    .TryGetSingleton(out AudioPlaybackState audioState) == false
                || audioState.Playback == null)
                return;

            AudioPlayback playback = audioState.Playback;

            state.Dependency.Complete();
            _eventsLookup.Update(ref state);

            foreach (var (
                         sfx,
                         modelLink,
                         transform)
                     in SystemAPI.Query<
                         RefRO<DeathSfx>,
                         RefRO<ModelLink>,
                         RefRO<LocalToWorld>>())
            {
                Entity actor = modelLink.ValueRO.Value;

                if (_eventsLookup.HasComponent(actor) == false
                    || _eventsLookup.IsComponentEnabled(actor) == false)
                    continue;

                Vector3 position = transform.ValueRO.Position;

                playback.Play(sfx.ValueRO.Value, position);
            }
        }
    }
}