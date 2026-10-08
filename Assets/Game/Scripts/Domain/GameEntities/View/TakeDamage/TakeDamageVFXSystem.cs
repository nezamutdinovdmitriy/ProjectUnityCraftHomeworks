using Game.Components;
using Unity.Entities;
using UnityEngine;
using static Unity.Entities.SystemAPI.ManagedAPI;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct TakeDamageVFXSystem : ISystem
    {
        private BufferLookup<TakeDamageEvent> _takeDamageEventLookup;

        public void OnCreate(ref SystemState state)
        {
            _takeDamageEventLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(isReadOnly: true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _takeDamageEventLookup.Update(ref state);

            foreach ((UnityEngineComponent<ParticleSystem> particleSystem,
                         RefRO<ModelLink> modelLink)
                     in SystemAPI.Query<
                         UnityEngineComponent<ParticleSystem>,
                         RefRO<ModelLink>>())
            {
                Entity model = modelLink.ValueRO.Value;
                
                if (_takeDamageEventLookup.TryGetBuffer(model, out DynamicBuffer<TakeDamageEvent> events) == false
                    || events.IsEmpty)
                    continue;
            
                particleSystem.Value.Play(withChildren: true);
            }
        }
    }
}