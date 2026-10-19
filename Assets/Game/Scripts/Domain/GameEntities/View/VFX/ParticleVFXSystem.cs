using Game.Components;
using Unity.Entities;
using UnityEngine;
using ManagedAPI = Unity.Entities.SystemAPI.ManagedAPI;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct ParticleVFXSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commands = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (
                         vfx,
                         parts,
                         root)
                     in SystemAPI.Query<
                             RefRW<ParticleVFX>,
                             DynamicBuffer<ParticleVFXPart>>()
                         .WithEntityAccess())
            {
                bool starting = !vfx.ValueRO.Started;
                bool anyAlive = false;

                foreach (ParticleVFXPart part in parts)
                {
                    ParticleSystem particles = ManagedAPI.GetComponent<ParticleSystem>(part.Value);

                    if (particles.gameObject.activeInHierarchy == false)
                        continue;

                    if (starting)
                    {
                        particles.Play(withChildren: false);
                    }
                    else if (particles.IsAlive(withChildren: false))
                    {
                        anyAlive = true;
                        break;
                    }
                }

                if (starting)
                {
                    vfx.ValueRW.Started = true;
                    continue;
                }
                
                if(anyAlive)
                    continue;
                
                commands.DestroyEntity(root);
            }
        }
    }
}