using Game.Components;
using Modules.AudioEvents;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct TakeDamageSfxSystem : ISystem
    {
        private BufferLookup<TakeDamageEvent> _eventsLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TakeDamageSfx>();
            
            _eventsLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            AudioSystem audioSystem = AudioSystem.Instance;

            if (audioSystem == null || !audioSystem.isActiveAndEnabled)
                return;

            state.Dependency.Complete();
            
            _eventsLookup.Update(ref state);
            
            foreach (var (
                         sfx,
                         model,
                         transform)
                     in SystemAPI.Query<
                             RefRO<TakeDamageSfx>,
                             RefRO<ModelLink>,
                             RefRO<LocalToWorld>>())
            {
                Entity actor = model.ValueRO.Value;

                if (_eventsLookup.TryGetBuffer(actor, out DynamicBuffer<TakeDamageEvent> events)
                    && events.IsEmpty == false)
                {
                    Vector3 position = transform.ValueRO.Position;
                    audioSystem.PlayEvent(sfx.ValueRO.Key, position);
                }
            }
        }
    }
}